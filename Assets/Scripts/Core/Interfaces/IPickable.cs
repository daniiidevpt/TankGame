using Core.ModulesSystem;

namespace Core.Interfaces
{
    public interface IPickable
    {
        void OnPickedUp(ModuleController collector);
    }
}
