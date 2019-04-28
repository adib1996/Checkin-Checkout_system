Class About
    'Closes form
    Private Sub CloseForm(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Content = Nothing
            End If
        Next
    End Sub
End Class
