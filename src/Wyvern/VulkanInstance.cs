using System.Runtime.InteropServices;
using System.Text;
using Vortice.Vulkan;

using static Vortice.Vulkan.Vulkan;

namespace Wyvern;

public struct VulkanInstance {
    internal VkInstance Handle;
    internal VkInstanceApi InstanceApi;
    
    private static readonly string[] PreferredValidationLayers = [
        "VK_LAYER_KHRONOS_validation"
    ];

    public unsafe VulkanInstance() {
        List<string> inputLayers = new();

#if DEBUG
        VkResult result = vkEnumerateInstanceLayerProperties(out uint propertyCount);

        if (result == VkResult.Success && propertyCount > 0) {
            Span<VkLayerProperties> globalLayers = new VkLayerProperties[propertyCount];

            result = vkEnumerateInstanceLayerProperties(globalLayers);

            if (result == VkResult.Success && ContainsAll(globalLayers, PreferredValidationLayers)) {
                inputLayers.AddRange(PreferredValidationLayers);
            }
        }
#endif
        
        List<VkUtf8String> requiredExtensions = new();

        if (inputLayers.Count > 0) {
            requiredExtensions.Add(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
        }
        
        var appInfo = new VkApplicationInfo {
            sType = VkStructureType.ApplicationInfo,
            pApplicationName = new VkUtf8ReadOnlyString("wyverntest"u8),
            applicationVersion = VkVersion.Version_1_4,
            pEngineName = new VkUtf8ReadOnlyString("wyverntest"u8),
            engineVersion = VkVersion.Version_1_4,
            apiVersion = VkVersion.Version_1_4,
        };
        
        using var enabledLayerNamesArray = new VkStringArray(inputLayers.ToArray());
        using var enabledExtensionNamesArray = new VkStringArray(requiredExtensions.ToArray()!);
        
        var instanceCreateInfo = new VkInstanceCreateInfo {
            sType = VkStructureType.InstanceCreateInfo,
            pApplicationInfo = &appInfo,
            enabledLayerCount = enabledLayerNamesArray.Length,
            ppEnabledLayerNames = enabledLayerNamesArray,
            enabledExtensionCount = enabledExtensionNamesArray.Length,
            ppEnabledExtensionNames = enabledExtensionNamesArray
        };
        
        vkCreateInstance(&instanceCreateInfo, null, out Handle);
        
        result.CheckResult();
        
        InstanceApi = GetApi(Handle);
        
#if DEBUG
        Console.WriteLine("enabled instance extensions:");
        foreach (var ext in requiredExtensions) {
            Console.WriteLine($" - {ext.ToString()}");
        }
#endif
    }
    
    private static unsafe bool ContainsAll(ReadOnlySpan<VkLayerProperties> available, ReadOnlySpan<string> required) {
        foreach (var req in required) {
            bool found = false;
            foreach (var av in available) {
                if (Marshal.PtrToStringAnsi((IntPtr)av.layerName) == req) {
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }
        return true;
    }
}