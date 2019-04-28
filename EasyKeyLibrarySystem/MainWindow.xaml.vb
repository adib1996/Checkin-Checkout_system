Imports System.Windows.Media.Animation

Class MainWindow
    Dim pinned As Boolean = False

    Private Sub Load(sender As Object, e As EventArgs) Handles MyBase.Loaded
        FormFrame.Navigate(New LoginForm)
    End Sub

    'Animations
    'Side Menu Fade
    Private Sub OpenSM(sender As Object, e As RoutedEventArgs)
        SMBackground.Visibility = Visibility.Visible

        Dim da As DoubleAnimation = New DoubleAnimation()
        da.To = 0.7
        da.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        SMBackground.BeginAnimation(OpacityProperty, da)
    End Sub
    Public Sub CloseSM(sender As Object, e As RoutedEventArgs)
        'Checks if it is pinned, if yes, no action is taken
        If pinned = False Then
            Dim CloseSideMenu As Storyboard = CType(FindResource("CloseSideMenu"), Storyboard)
            CloseSideMenu.Begin()

            Dim da As DoubleAnimation = New DoubleAnimation()
            da.To = 0
            da.Duration = New Duration(TimeSpan.FromSeconds(0.3))
            SMBackground.BeginAnimation(OpacityProperty, da)

            SMBackground.Visibility = Visibility.Collapsed
        End If
    End Sub

    Public Sub PinSM(sender As Object, e As RoutedEventArgs)
        'To tell the application that the side menu is pinned, this help prevent closing the side menu when CloseSM is called
        pinned = True

        'Lightens up the background
        Dim da As DoubleAnimation = New DoubleAnimation()
        da.To = 0
        da.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        SMBackground.BeginAnimation(OpacityProperty, da)
        SMBackground.Visibility = Visibility.Hidden

        'Sets the margin of the contents to give space for the side menu
        ContentFrame.Margin = New Thickness(200, 50, 0, 0)
        TitleBar.Margin = New Thickness(200, 0, 0, 0)

        'Switches the pin button to unpin button
        SMPinButton.Visibility = Visibility.Collapsed
        SMUnpinButton.Visibility = Visibility.Visible

        'Hides the close side menu button
        SMCloseButton.Visibility = Visibility.Hidden

        'Increases the application width to allow more space for the content
        If Application.Current.MainWindow.Width < 900 Then
            Application.Current.MainWindow.Width = 900
        End If
    End Sub

    Public Sub UnpinSM(sender As Object, e As RoutedEventArgs)
        'To tell the application that the side menu is no longer pinned
        pinned = False

        'Darkens the background like it would when side menu is opened but not pinned
        OpenSM(Me, New RoutedEventArgs)

        'Sets the margin of the contents to give space for the side menu
        ContentFrame.Margin = New Thickness(0, 40, 0, 0)
        TitleBar.Margin = New Thickness(0, 0, 0, 0)

        'Switches the unpin button to pin button
        SMUnpinButton.Visibility = Visibility.Collapsed
        SMPinButton.Visibility = Visibility.Visible

        'Shows the close side menu button
        SMCloseButton.Visibility = Visibility.Visible

        'Decreases the application width if it is the standard unajusted width set when side menu is pinned
        If Application.Current.MainWindow.Width = 900 Then
            Application.Current.MainWindow.Width = 700
        End If
    End Sub

    'Window Shell Controls
    Private Sub Drag(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Mouse.LeftButton = MouseButtonState.Pressed Then
            DragMove()
        End If
    End Sub
    Private Sub Maximize(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Maximized

        MaximizeButton.Visibility = Visibility.Collapsed
        RestoreButton.Visibility = Visibility.Visible
    End Sub
    Private Sub Restore(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Normal

        MaximizeButton.Visibility = Visibility.Visible
        RestoreButton.Visibility = Visibility.Collapsed
    End Sub
    Private Sub Minimize(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub
    Private Sub ExitApplication(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub
End Class
