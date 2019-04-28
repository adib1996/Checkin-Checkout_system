Class Help
    Private Sub About(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New About)
            End If
        Next
    End Sub
    Private Sub Feedback(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New Feedback)
            End If
        Next
    End Sub
End Class
