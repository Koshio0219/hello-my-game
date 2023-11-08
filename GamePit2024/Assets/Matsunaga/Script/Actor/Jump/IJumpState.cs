public interface IJumpState
{
    bool _isPressed { get; }
    JumpState stay_update();    // ��Ԃɗ��܂��Ă����Update�Ŏ��s�����
    void stay_fixed_update();    // ��Ԃɗ��܂��Ă����FixedUpdate�Ŏ��s�����
    void enter();    // ��ԂɑJ�ڂ��Ă������Ɉ�x�������s�����
    void exit();    // ��Ԃ���J�ڂ��Ă������Ɉ�x�������s�����
    void SetPressed(bool value);
}
