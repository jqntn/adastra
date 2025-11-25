#include <cstdint>
#include <raylib.h>

constexpr uint16_t REF_RES = 1600;
constexpr float REF_DPI = 1.5f;

static float
get_scaling_factor()
{
  return static_cast<float>(GetScreenHeight()) / REF_RES *
         GetWindowScaleDPI().y / REF_DPI;
}

int
main()
{
  SetConfigFlags(FLAG_VSYNC_HINT | FLAG_MSAA_4X_HINT | FLAG_WINDOW_UNDECORATED);
  InitWindow(0, 0, "Ad Astra");
  SetWindowSize(GetScreenWidth() + 1, GetScreenHeight() + 1);
  SetExitKey(KEY_NULL);
  InitAudioDevice();

  while (!WindowShouldClose()) {
    BeginDrawing();
    ClearBackground(BLACK);
    EndDrawing();
  }

  CloseAudioDevice();
  CloseWindow();
}