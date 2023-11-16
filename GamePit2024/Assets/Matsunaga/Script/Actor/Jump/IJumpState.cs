﻿public interface IJumpState
{
    JumpState stay_update();    // 状態に留まっている間Updateで実行される
    void stay_fixed_update();    // 状態に留まっている間FixedUpdateで実行される
    void enter();    // 状態に遷移してきた時に一度だけ実行される
    void exit();    // 状態から遷移していく時に一度だけ実行される
    void SetGamepadNumber(int _GamepadNumber);
}
