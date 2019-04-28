Class Feedback

    Private Sub SubmitFeedback(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Feature is currently being developed, and is unavailable")
    End Sub

    'Closes form
    Private Sub CloseForm(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Content = Nothing
            End If
        Next
    End Sub
End Class
