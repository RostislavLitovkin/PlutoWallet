using PlutoWallet.Model;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.Events;

public partial class EventItemView : ContentView
{
    public static readonly BindableProperty PalletNameProperty = BindableProperty.Create(
        nameof(PalletName), typeof(string), typeof(EventItemView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventItemView)bindable;

            control.palletLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
        nameof(EventName), typeof(string), typeof(EventItemView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventItemView)bindable;

            control.eventLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ParametersProperty = BindableProperty.Create(
        nameof(Parameters), typeof(List<EventParameter>), typeof(EventItemView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventItemView)bindable;


            var parametersList = (List<EventParameter>)(newValue);

            foreach (var parameter in parametersList)
            {
                control.eventParametersStackLayout.Add(new EventParameterView
                {
                    Name = parameter.Name,
                    Value = parameter.Value
                });
            }

            if (!parametersList.Any())
            {
                control.eventParametersStackLayout.Margin = new Thickness(0, 0, 0, 5);
            }
        });

    public static readonly BindableProperty SafetyProperty = BindableProperty.Create(
        nameof(Safety), typeof(EventSafety), typeof(EventItemView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventItemView)bindable;

            control.safetyLabel.Text = ((EventSafety)newValue).ToString();

            switch ((EventSafety)newValue)
            {
                case EventSafety.Safe:
                    control.safetyLabel.TextColor = Colors.Green;
                    break;
                case EventSafety.Ok:
                    control.safetyLabel.TextColor = Colors.Gray;
                    break;
                case EventSafety.Unknown:
                    control.safetyLabel.TextColor = Colors.Gray;
                    break;
                case EventSafety.Warning:
                    control.safetyLabel.TextColor = Colors.DarkOrange;
                    break;
                case EventSafety.Harmful:
                    control.safetyLabel.TextColor = Colors.DarkRed;
                    break;
            }
        });
    public EventItemView()
    {
        InitializeComponent();
    }

    public string PalletName
    {
        get => (string)GetValue(PalletNameProperty);

        set => SetValue(PalletNameProperty, value);
    }

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);

        set => SetValue(EventNameProperty, value);
    }

    public List<EventParameter> Parameters
    {
        get => (List<EventParameter>)GetValue(ParametersProperty);

        set => SetValue(ParametersProperty, value);
    }

    public EventSafety Safety
    {
        get => (EventSafety)GetValue(SafetyProperty);
        set => SetValue(SafetyProperty, value);
    }
}