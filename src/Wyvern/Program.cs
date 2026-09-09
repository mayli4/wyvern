using SDL3;
using Vortice.Vulkan;
using Wyvern;

file static class Program {
    public static void Main() {
        var app = new Main();
        app.Init();
    }
}

internal class Main {
    public VulkanInstance Instance;
    
    public void Init() {
        SDL.Init(SDL.InitFlags.Video);
        var window = new Window("wyvern", 800, 800, new Window.Callbacks());

        Vulkan.vkInitialize().CheckResult();

        Instance = new();
        
        while(window.PollEvents()) {
    
        }
    }
}