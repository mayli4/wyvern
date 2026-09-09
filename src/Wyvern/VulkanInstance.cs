using System.Text;
using Vortice.Vulkan;

using static Vortice.Vulkan.Vulkan;

namespace Wyvern;

public struct VulkanInstance {
    internal VkInstance Handle;
    internal VkInstanceApi InstanceApi;

    public unsafe VulkanInstance() {
        var appInfo = new VkApplicationInfo {
            sType = VkStructureType.ApplicationInfo,
            pApplicationName = new VkUtf8ReadOnlyString("wyverntest"u8),
            applicationVersion = VkVersion.Version_1_4,
            pEngineName = new VkUtf8ReadOnlyString("wyverntest"u8),
            engineVersion = VkVersion.Version_1_4,
            apiVersion = VkVersion.Version_1_4,
        };
        
        var instanceCreateInfo = new VkInstanceCreateInfo {
            sType = VkStructureType.InstanceCreateInfo,
            pApplicationInfo = &appInfo,
            // enabledLayerCount = (uint)inputLayers.Count,
            // ppEnabledLayerNames = new VkStringArray(inputLayers)
        };
        
        var result = vkCreateInstance(&instanceCreateInfo, null, out Handle);
        
        result.CheckResult();
        
        InstanceApi = GetApi(Handle);
    }
}