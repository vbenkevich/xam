using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using NLib.UI;

namespace NLib.iOS.Navigation
{
    public class NavigationMap
    {
        public static readonly NavigationMap Instance = new NavigationMap();

        private readonly Dictionary<Type, List<Tuple<Type, string>>> seguesMap;
        private readonly Dictionary<Type, Type> controllersMap;

        public NavigationMap()
        {
            seguesMap = new Dictionary<Type, List<Tuple<Type, string>>>();
            controllersMap = new Dictionary<Type, Type>();
        }

        public void RegisterSegue<TSource, TViewModel>(string segueId)
            where TSource : UIViewController
            where TViewModel : ViewModel
        {
            if (!seguesMap.TryGetValue(typeof(TViewModel), out List<Tuple<Type, string>> list))
            {
                list = new List<Tuple<Type, string>>();
                seguesMap.Add(typeof(TViewModel), list);
            }

            list.Add(Tuple.Create(typeof(TSource), segueId));
        }

        public bool TryGetSegueId<TViewModel>(UIViewController source, out string segueId)
        {
            segueId = null;

            if (!seguesMap.TryGetValue(typeof(TViewModel), out List<Tuple<Type, string>> list))
                return false;

            segueId = list.FirstOrDefault(t => t.Item1 == source.GetType())?.Item2;

            return segueId != null;
        }

        public void RegisterController<TViewModel,TController>()
            where TController : UIViewController, IViewController<TViewModel>
            where TViewModel : ViewModel
        {
            controllersMap[typeof(TViewModel)] = typeof(TController);
        }

        public UIViewController CreateViewController<TViewModel>() 
            where TViewModel : ViewModel
        {
            return (UIViewController)Activator.CreateInstance(controllersMap[typeof(TViewModel)]);
        }
    }
}
