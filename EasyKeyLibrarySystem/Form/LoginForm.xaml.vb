Imports System.Data.OleDb
Class LoginForm
    Private Sub Login(sender As Object, e As RoutedEventArgs)
        'Failsafe as passwords are encrypted and cannot be recovered
        If LoginTPNumber.Text = "EZKLBSFS" And LoginPassword.Password = "EZKLBSFSAdmin" Then
            'Searches the required frames to bring forth the Administrator page
            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).ContentFrame.Navigate(New Reports)
                End If
            Next

            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).SideMenuFrame.Navigate(New AdminSM)
                End If
            Next

            'Copies details to side menu profile
            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).SMRole.Text = "Owner"
                End If
            Next
            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).SMName.Text = "Golden Wells"
                End If
            Next

            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).SMTPNumber.Text = "Fail Safe"
                End If
            Next

            'Closes the Login page by clearing the frame
            For Each window As Window In Application.Current.Windows
                If (window.GetType = GetType(MainWindow)) Then
                    CType(window, MainWindow).FormFrame.Content = Nothing
                End If
            Next
            LoginTPNumber.Text = ""
            LoginPassword.Password = ""
        Else
            Try
                'Connection to database to retrieve salt
                Con.Open()

                Com = New OleDbCommand("SELECT TPNumber, Salt FROM RegisteredStaff WHERE TPNumber ='" + LoginTPNumber.Text + "'", Con)

                Reader = Com.ExecuteReader()
                Reader.Read()

                If (Reader.HasRows = False) Then
                    MessageBox.Show("Invalid TP Number and/or Password" + vbCrLf + "Error Code: ELOG001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else
                    'Password Encryption
                    Dim password = Encryption.HashString(LoginPassword.Password)
                    Dim salt = Reader.Item("Salt")
                    Dim hashedandsalted = Encryption.HashString(String.Format("{0}{1}", salt, password))

                    'Cross-reference TP Number and hashed password enteres
                    Com = New OleDbCommand("SELECT [TPNumber], [Name], [Password], [Role] FROM RegisteredStaff WHERE `TPNumber` ='" + LoginTPNumber.Text + "' AND `Password` ='" + hashedandsalted + "'", Con)

                    Reader = Com.ExecuteReader()
                    Reader.Read()

                    'Determines the role and setups the right page
                    If (Reader.HasRows = False) Then
                        MessageBox.Show("Invalid TP Number and/or Password" + vbCrLf + "Error Code: ELOG001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Else
                        'Edits the TP Number
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).SMTPNumber.Text = LoginTPNumber.Text
                            End If
                        Next

                        'If user is an administrator
                        If (Reader.Item("Role") = "Administrator") Then
                            'Searches the required frames to bring forth the Administrator page
                            For Each window As Window In Application.Current.Windows
                                If (window.GetType = GetType(MainWindow)) Then
                                    CType(window, MainWindow).ContentFrame.Navigate(New Reports)
                                End If
                            Next

                            For Each window As Window In Application.Current.Windows
                                If (window.GetType = GetType(MainWindow)) Then
                                    CType(window, MainWindow).SideMenuFrame.Navigate(New AdminSM)
                                End If
                            Next

                            'If user is an Library Assistant or Trainee
                        ElseIf (Reader.Item("Role") = "Library Assistant") Or (Reader.Item("Role") = "Trainee") Then
                            'Searches the required frames to bring forth the Library Assistant page
                            For Each window As Window In Application.Current.Windows
                                If (window.GetType = GetType(MainWindow)) Then
                                    CType(window, MainWindow).ContentFrame.Navigate(New Clock)
                                End If
                            Next

                            For Each window As Window In Application.Current.Windows
                                If (window.GetType = GetType(MainWindow)) Then
                                    CType(window, MainWindow).SideMenuFrame.Navigate(New LibrarianSM)
                                End If
                            Next
                        End If

                        'Copies details to side menu profile
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).SMRole.Text = Reader.Item("Role")
                            End If
                        Next
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).SMName.Text = Reader.Item("Name")
                            End If
                        Next

                        'Closes the Login page by clearing the frame
                        For Each window As Window In Application.Current.Windows
                            If (window.GetType = GetType(MainWindow)) Then
                                CType(window, MainWindow).FormFrame.Content = Nothing
                            End If
                        Next
                        LoginTPNumber.Text = ""
                        LoginPassword.Password = ""
                    End If
                End If
                Con.Close()
            Catch edtb As OleDbException
                MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Exit Try
            End Try
        End If
    End Sub

    'Hotkey enter to login
    Private Sub LoginHotkey(sender As Object, e As KeyEventArgs)
        'Hotkey Enter
        If e.Key = Key.Enter Then
            Login(Me, New RoutedEventArgs)
        End If
    End Sub

    'Windows Shell Functions
    Private Sub ExitApplication(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub
End Class
