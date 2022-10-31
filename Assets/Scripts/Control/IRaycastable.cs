namespace RPG.Control
{
    public interface IRaycastable 
    {
        CursorType_SO GetCursorType();
        bool HandleRaycast(PlayerController callingController);
    }
}
