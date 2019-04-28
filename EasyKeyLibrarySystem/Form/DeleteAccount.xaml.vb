Imports System.Data.OleDb

Class DeleteAccount
    Private Sub Load(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        'Connection to database
        Con.Open()
        Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff", Con)

        Reader = Com.ExecuteReader()

        'To populate the combobox
        Try
            Do
                Reader.Read()
                Dim TPNumbers As String = Reader.Item("TPNumber")
                DeleteTPNumber.Items.Add(TPNumbers)
            Loop
        Catch ex As InvalidOperationException
            'To stop loop when there is no rows
            Exit Try
        End Try
        Con.Close()

        'Retrieves the TP Number
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                TPNumber.Text = CType(window, MainWindow).SMTPNumber.Text
            End If
        Next
    End Sub

    Private Sub DeleteAccount(sender As Object, e As RoutedEventArgs)
        If DeleteTPNumber.SelectedValue = "" Or DeletePassword.Password = "" Then
            MessageBox.Show("Please fill in all the required information")
        Else
            'Prevents the deletion of the account currently being used
            If Not TPNumber.Text = DeleteTPNumber.SelectedValue Then
                'Password Encryption
                Dim password = Encryption.HashString(DeletePassword.Password)
                Dim salt = Encryption.GenerateSalt
                Dim hashedandsalted = Encryption.HashString(String.Format("{0}{1}", salt, password))

                'Connection to database
                Con.Open()

                Com = New OleDbCommand("SELECT [TPNumber], [Password] FROM RegisteredStaff WHERE TPNumber ='" + TPNumber.Text + "' Password ='" + hashedandsalted + "'", Con)

                Reader = Com.ExecuteReader()
                Reader.Read()

                'Validate administrative password
                If (Reader.HasRows = False) Then
                    MessageBox.Show("Incorrect admin password")
                Else
                    Dim dialog As MessageBoxResult
                    dialog = MessageBox.Show("Comfirm the deletion of account " + DeleteTPNumber.SelectedValue + "?", "Notice", MessageBoxButton.YesNo)

                    If dialog = MessageBoxResult.Yes Then
                        'Connection to database
                        Com = New OleDbCommand("DELETE FROM RegisteredStaff WHERE [TPNumber] ='" + DeleteTPNumber.SelectedValue + "'", Con)

                        Reader = Com.ExecuteReader()
                        Reader.Read()

                        MessageBox.Show("Successful!")

                        'Closes Delete Account page
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).FormFrame.Content = Nothing
                            End If
                        Next

                        'Reloads Account page
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).ContentFrame.Navigate(New Accounts)
                            End If
                        Next
                    ElseIf dialog = MessageBoxResult.No Then
                        MessageBox.Show("Cancelled")
                    End If
                End If
                Con.Close()
            Else
                MessageBox.Show("You cannot delete this account while using it, please log in with another Administrator account to do so.")
            End If
        End If
    End Sub

    'Prevents the deletion of the account currently being used
    Private Sub TPNumberValidator(sender As Object, e As SelectionChangedEventArgs)
        If TPNumber.Text = DeleteTPNumber.SelectedValue Then
            CurrentAdmin.Visibility = Visibility.Visible
        Else
            CurrentAdmin.Visibility = Visibility.Hidden
        End If
    End Sub

    'Hotkey enter to delete
    Private Sub DeleteHotkey(sender As Object, e As KeyEventArgs)
        'Hotkey Enter
        If e.Key = Key.Enter Then
            DeleteAccount(Me, New RoutedEventArgs)
        End If
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
