
namespace ASRMenuFactory
{
    /* Abstract class Menu Factory to implement abstract method get menu type
    */

    abstract class AbstractMenuFactory
    {
        public abstract IMenu GetMenu(string menuName);
        
    }
}
