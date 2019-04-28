Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Class AddAccount
    Private Sub AddAccount(sender As Object, e As RoutedEventArgs)
        'Checks if any field is unfilled
        If AddName.Text = "" Or AddTPNumber.Text = "" Or AddPassword.Password = "" Or AddReconfirmPassword.Password = "" Or AddContact.Text = "" Or AddEmail.Text = "" Or AddRole.Text = "" Then
            MessageBox.Show("Please fill in all the required information")
        Else
            'Connection to database
            Con.Open()

            Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff WHERE TPNumber ='" + AddTPNumber.Text + "'", Con)

            Reader = Com.ExecuteReader()
            Reader.Read()

            'Checks if TP Number existed in database
            If (Reader.HasRows = False) Then
                'Checks if password has a minimum of 8 characters
                If AddPassword.Password.Length > 7 Then
                    'Warns if TP Number is the same as the password
                    If AddPassword.Password = AddTPNumber.Text Then
                        MessageBox.Show("Your TP Number and Password are the same. This will severely weaken the security." + vbCrLf + "Warning Code: WACC001", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning)
                    End If
                    'Checks if password is same with reconfirm password
                    If AddPassword.Password = AddReconfirmPassword.Password Then
                        'Password Encryption
                        Dim password = Encryption.HashString(AddPassword.Password)
                        Dim salt = Encryption.GenerateSalt
                        Dim hashedandsalted = Encryption.HashString(String.Format("{0}{1}", salt, password))


                        'Connection to database
                        Com = New OleDbCommand("INSERT INTO `RegisteredStaff` (`TPNumber`, `Name`, `Password`, `Contact`, `Email`, `Role`, `DateRegistered`, `Salt`) VALUES (@TPNumber, @Name, @Password, @Contact, @Email, @Role, @DateRegistered, @Salt)", Con)

                        Com.Parameters.AddWithValue("@TPNumber", AddTPNumber.Text)
                        Com.Parameters.AddWithValue("@Name", AddName.Text)
                        Com.Parameters.AddWithValue("@Password", hashedandsalted)
                        Com.Parameters.AddWithValue("@Contact", AddContact.Text)
                        Com.Parameters.AddWithValue("@Email", AddEmail.Text)
                        Com.Parameters.AddWithValue("@Role", AddRole.Text)
                        Com.Parameters.AddWithValue("@DateRegistered", CurrentDateTime)
                        Com.Parameters.AddWithValue("@Salt", salt)

                        Com.ExecuteNonQuery()

                        Dim addedData As Boolean = True

                        If addedData Then
                            AddName.Clear()
                            AddTPNumber.Clear()
                            AddPassword.Clear()
                            AddReconfirmPassword.Clear()
                            AddContact.Clear()
                            AddEmail.Clear()
                            AddRole.Text = ""
                            MessageBox.Show("Successful!")

                            'Closes Add Account Page
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
                        End If
                        addedData = False
                    Else
                        MessageBox.Show("Passwords do not match" + vbCrLf + "Error Code: EACC003", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    End If
                Else
                    MessageBox.Show("Password has insufficient characters" + vbCrLf + "Error Code: EACC002", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                End If
            Else
                MessageBox.Show("TP Number already existed" + vbCrLf + "Error Code: EACC001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
            Con.Close()
        End If
    End Sub

    'Ensures TP Number does not exist in database
    Private Sub TPNumberValidation(sender As Object, e As RoutedEventArgs)
        If AddTPNumber.Text = "" Then

        Else
            'Connection to database
            Con.Open()

            Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff WHERE TPNumber ='" + AddTPNumber.Text + "'", Con)

            Reader = Com.ExecuteReader()
            Reader.Read()

            If (Reader.HasRows = True) Then
                TPNumberExisted.Visibility = Visibility.Visible
            Else
                TPNumberExisted.Visibility = Visibility.Hidden
            End If
        End If
        Con.Close()
    End Sub

    'Ensures only numbers and certain symbols are given
    Private Sub ContactNumberValidation(sender As Object, e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("[^0-9-+]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub PasswordValidation(sender As Object, e As RoutedEventArgs)
        'Ensures both password are the same
        If AddPassword.Password = AddReconfirmPassword.Password Then
            DoNotMatch.Visibility = Visibility.Hidden
        Else
            DoNotMatch.Visibility = Visibility.Visible
        End If

        'Ensures password has a minimum of 8 characters
        If AddPassword.Password.Length < 8 Then
            InsufficientCharacters.Visibility = Visibility.Visible
        Else
            InsufficientCharacters.Visibility = Visibility.Collapsed
        End If

        'Warns if TP Number and the password are the same
        If AddPassword.Password = AddTPNumber.Text Then
            SameTPNumber.Visibility = Visibility.Visible
        Else
            SameTPNumber.Visibility = Visibility.Hidden
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
