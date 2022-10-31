using System;
namespace Maui.FreakyControls.Shared.Delegates
{
    public delegate void CardsViewItemAppearingHandler(CardsView view, ItemAppearingEventArgs args);
    public delegate void CardsViewItemAppearedHandler(CardsView view, ItemAppearedEventArgs args);
    public delegate void CardsViewItemDisappearingHandler(CardsView view, ItemDisappearingEventArgs args);
    public delegate void CardsViewItemSwipedHandler(CardsView view, ItemSwipedEventArgs args);
    public delegate void CardsViewUserInteractedHandler(CardsView view, UserInteractedEventArgs args);
}

