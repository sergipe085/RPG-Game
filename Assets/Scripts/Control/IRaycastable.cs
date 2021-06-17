using RPG.Combat;

namespace RPG.Control
{
    public interface IRaycastable {
        bool HandleRaycast(PlayerController controller);
    }
}
