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

        public bool TryPerformSegue<TViewModel>(UIViewController source)
        {
            if (!seguesMap.TryGetValue(typeof(TViewModel), out List<Tuple<Type, string>> list))
                return false;

            var pair = list.FirstOrDefault(t => t.Item1 == source.GetType());

            if (pair != null)
            {
                source.PerformSegue(pair.Item2, source);
                return true;
            }

            return false;
        }
    }
}
