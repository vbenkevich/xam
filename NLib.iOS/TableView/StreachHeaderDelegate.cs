using System;
using UIKit;
using CoreGraphics;

namespace NLib.iOS.TableView
{
    class StreachHeaderDelegate : UITableViewDelegate
    {
        readonly UIView headerView;
        readonly nfloat defaultHeight;
        readonly nfloat compactHeight;

        private nfloat lastOffset;

        public StreachHeaderDelegate(UITableView tableView, float compactHeight = 0f)
        {
            if (tableView.TableHeaderView == null) return;

            this.compactHeight = compactHeight;

            headerView = tableView.TableHeaderView;
            tableView.TableHeaderView = null;
            tableView.Superview.AddSubview(headerView);

            defaultHeight = headerView.Frame.Height;

            var oldContentInset = tableView.ContentInset;
            tableView.ContentInset = new UIEdgeInsets(defaultHeight + oldContentInset.Top, oldContentInset.Left, oldContentInset.Bottom, oldContentInset.Right);
            tableView.ContentOffset = new CGPoint(0, -defaultHeight);

            lastOffset = tableView.ContentOffset.Y;

            UpdateHeaderFrame(tableView);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            UpdateHeaderFrame(scrollView);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            OnScrollingStopped(scrollView);
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (!willDecelerate)
            {
                OnScrollingStopped(scrollView);
            }
        }

        // todo not working animation + buggy at top
        private void OnScrollingStopped(UIScrollView scrollView)
        {
            return;

            var newHeight = headerView.Frame.Height > (defaultHeight + compactHeight) / 2 ? defaultHeight : compactHeight;

            if (!EuqalSize(newHeight, headerView.Frame.Height))
            {
                UIView.Animate(350f, () =>
                {
                    headerView.Frame = MakeFrame(newHeight, scrollView);
                    headerView.LayoutIfNeeded();
                });
            }
        }

        private void UpdateHeaderFrame(UIScrollView scrollView)
        {
            if (headerView == null) return;

            var newHeight = Max(compactHeight, -scrollView.ContentOffset.Y);
            var dy = scrollView.ContentOffset.Y - lastOffset;

            if (dy > 0 && headerView.Frame.Height > compactHeight && scrollView.ContentOffset.Y > 0)
            {
                newHeight = Max(compactHeight, headerView.Frame.Height - dy);
            }
            else if (dy < 0 && scrollView.ContentOffset.Y < scrollView.ContentSize.Height - scrollView.Frame.Height && newHeight < defaultHeight)
            {
                newHeight = Min(defaultHeight, headerView.Frame.Height - dy);
            }

            headerView.Frame = MakeFrame(newHeight, scrollView);

            lastOffset = scrollView.ContentOffset.Y;

            headerView.LayoutIfNeeded();
        }

        private CGRect MakeFrame(nfloat newHeight, UIScrollView scrollView)
        {
            return new CGRect(scrollView.Frame.Left, scrollView.Frame.Top, scrollView.Bounds.Width, newHeight);
        }

        private bool EuqalSize(nfloat a, nfloat b)
        {
            var diff = a - b;
            return diff < 1f && diff > -1f;
        }

        private nfloat Max(nfloat a, nfloat b)
        {
            if (a < b) return b;
            return a;
        }

        private nfloat Min(nfloat a, nfloat b)
        {
            if (a > b) return b;
            return a;
        }
    }
}
