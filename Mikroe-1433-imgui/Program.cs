using ImGuiNET;
using Mikroe.Imgui;
using System.Diagnostics;
using System.Numerics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
Sdl2Window _window;
GraphicsDevice _gd;
CommandList _cl;
Vector3 _clearColor = new Vector3(0.0f, 0.0f, 0.6f);
ImGuiController _controller;
VeldridStartup.CreateWindowAndGraphicsDevice(
    new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal,string.Empty),
    new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
    out _window,
    out _gd);
_controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);
_window.Resized += () =>
{
    _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
    _controller.WindowResized(_window.Width, _window.Height);
};
_cl = _gd.ResourceFactory.CreateCommandList();
var stopwatch = Stopwatch.StartNew();
float deltaTime = 0f;
while (_window.Exists)
{
    deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
    stopwatch.Restart();
    InputSnapshot snapshot = _window.PumpEvents();
    if (!_window.Exists) { break; }
    _controller.Update(deltaTime, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.
    if (ImGui.Begin("Mikroe-1433 demo",ImGuiWindowFlags.DockNodeHost))
    {
        float[] vs = new float[] { 1.0f, 2.0f, 3.0f, 0.5f };
        double v = 0.1;
        
        ImGui.SeparatorText("Readings");
        
        ImGui.Text($"A/D Converter");
        ImGui.SameLine(0, 10);
        ImGui.Text($"{v}");
        ImGui.SameLine(0, 10);
        ImGui.PlotLines("A/D",ref vs[0], vs.Length);
        ImGui.Text($"CO²");
        ImGui.SameLine(0, 10);
        ImGui.Text($"{v} ppm");
        ImGui.Text($"Temperature");
        ImGui.SameLine(0, 10);
        ImGui.Text($"{v} °C");
        ImGui.Text($"RH");
        ImGui.SameLine(0, 10);
        ImGui.Text($"{v} %%");
        ImGui.Separator();
    }
    ImGui.End();

    _cl.Begin();
    _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
    _cl.ClearColorTarget(0, RgbaFloat.Clear);// new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 0f));
    _controller.Render(_gd, _cl);
    _cl.End();
    _gd.SubmitCommands(_cl);
    _gd.SwapBuffers(_gd.MainSwapchain);
}
