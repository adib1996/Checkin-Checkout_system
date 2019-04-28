Imports System.Data
Imports System.Data.OleDb
Class Accounts
    Private Sub Load(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        'Connection to the database
        Con.Open()

        Com = New OleDbCommand("SELECT * FROM RegisteredStaff", Con)

        Dim dataAdapter As New OleDbDataAdapter(Com)
        Dim datatable As New DataTable
        dataAdapter.Fill(datatable)
        AccountsDataGrid.ItemsSource = datatable.AsDataView
        Con.Close()
    End Sub

    Private Sub AddAccount(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New AddAccount)
            End If
        Next
    End Sub

    Private Sub EditAccount(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New EditAccount)
            End If
        Next
    End Sub

    Private Sub DeleteAccount(sender As Object, e As RoutedEventArgs)
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                CType(window, MainWindow).FormFrame.Navigate(New DeleteAccount)
            End If
        Next
    End Sub
End Class
