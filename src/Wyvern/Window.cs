using SDL3;

namespace Wyvern;

public struct Window {
    public delegate void WindowCallback(Window window);
    public delegate void ResizeCallback(Window window, u32 width, u32 height);
    public delegate void MouseWheelCallback(Window window, f32 delta);
    
    public readonly record struct Callbacks(
        WindowCallback? OnClose, 
        WindowCallback? OnCreation, 
        ResizeCallback? OnResize
        ) {
        public MouseWheelCallback? OnMouseWheel { get; init; }
    }
    
    internal isize Handle;

    public readonly string Title;

    public u32 Width, Height;
    
    private readonly Callbacks callbacks;
    
    public Window(ReadOnlySpan<char> title, i32 width, i32 height, Callbacks callbacks) {
        this.callbacks = callbacks;
        Title = title.ToString();
    
        Handle = SDL.CreateWindow(Title, width, height, SDL.WindowFlags.Resizable);

        SDL.GetWindowSizeInPixels(Handle, out i32 w, out i32 h);
        Width = (u32)w;
        Height = (u32)h;
    }

    public bool PollEvents() {
        bool shouldContinue = true;
        
        while(SDL.PollEvent(out var @event)) {
            if(@event.Type == (u32)SDL.EventType.WindowResized) {
                var windowEvent = @event.Window;
                callbacks.OnResize?.Invoke(this, (u32)windowEvent.Data1, (u32)windowEvent.Data2);
            }

            if (@event.Type == (u32)SDL.EventType.MouseWheel) {
                callbacks.OnMouseWheel?.Invoke(this, @event.Wheel.Y);
            }
            
            if(@event.Type == (u32)SDL.EventType.Quit) {
                callbacks.OnClose?.Invoke(this);
                shouldContinue = false;
            }
        }

        return shouldContinue;
    }
    
    public static implicit operator isize(Window window) => window.Handle;
}
