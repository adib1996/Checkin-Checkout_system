Imports System.Windows.Media.Animation

Class AdminSM

    Private Sub OpenReports(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).ContentFrame.Navigate(New Reports)
            End If
        Next

        'Closes side menu
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).CloseSM(Me, New RoutedEventArgs)
            End If
        Next
    End Sub

    Private Sub OpenAccounts(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).ContentFrame.Navigate(New Accounts)
            End If
        Next

        'Closes side menu
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).CloseSM(Me, New RoutedEventArgs)
            End If
        Next
    End Sub

    Private Sub OpenHelp(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).ContentFrame.Navigate(New Help)
            End If
        Next

        'Closes side menu
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).CloseSM(Me, New RoutedEventArgs)
            End If
        Next
    End Sub

    Private Sub LogOut(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).ContentFrame.Content = Nothing
            End If
        Next

        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New LoginForm)
            End If
        Next

        'Closes side menu
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).CloseSM(Me, New RoutedEventArgs)
            End If
        Next
    End Sub
End Class
