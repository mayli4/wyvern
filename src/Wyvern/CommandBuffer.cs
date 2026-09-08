using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Wyvern;

public struct CommandBuffer(VkCommandBuffer handle) {
    internal VkCommandBuffer Handle = handle;
    
    public void Begin()
    {
        
    }
}