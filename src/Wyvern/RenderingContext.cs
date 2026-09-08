namespace Wyvern;

internal enum BackendType : u8 {
    Vulkan,
    Dx12,
    None,
}

internal ref struct ContextDesc {
    
}

internal interface IRenderingContext;

public sealed class RenderingContext {
    public RenderingContext() {
        
    }
}