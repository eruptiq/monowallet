using MvvmCross.ReactiveUI.Interop;
using MvvmCross.ViewModels;

namespace Monowallet.UI.Core.ViewModels.Base
{
    public class ViewModelBase<TParam> : MvxReactiveViewModel<TParam>, IBaseViewModel
    {
        //public abstract new string Icon { get; }

        //public abstract new string Title { get; }

        protected TParam Parameter { get; set; }

        public override void Prepare(TParam parameter)
        {
            this.Parameter = parameter;
        }

        private string icon;
        private bool isBusy;
        private string title;


        public string Icon
        {
            get { return icon; }
            set
            {
                this.RaiseAndSetIfChanged(ref icon, value);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                this.RaiseAndSetIfChanged(ref isBusy, value);
            }
        }
    }


    public class ViewModelBase : MvxReactiveViewModel, IBaseViewModel
    {
        private string icon;
        private bool isBusy;
        private string title;


        public string Icon
        {
            get { return icon; }
            set
            {
                this.RaiseAndSetIfChanged(ref icon, value);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                this.RaiseAndSetIfChanged(ref isBusy, value);
            }
        }
    }
}