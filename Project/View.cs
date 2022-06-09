using System.Windows.Forms;

namespace Project
{
    public abstract class View : PictureBox 
    {
        protected View()
        {
            
        }

        protected abstract void SetComponents();

        public void ResizeComponents()
        {
            // Update component sizes and locations when the BaseForm is resized
            this.SetComponents();
        }
    }
}