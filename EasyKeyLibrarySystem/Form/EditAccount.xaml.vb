Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Class EditAccount
    Private Sub Load(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        Try
            'Connection to database
            Con.Open()
            Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff", Con)

            Reader = Com.ExecuteReader()

            'To populate the combobox
            Try
                Do
                    Reader.Read()
                    Dim TPNumbers As String = Reader.Item("TPNumber")
                    EditChooseTPNumber.Items.Add(TPNumbers)
                Loop
            Catch ex As InvalidOperationException
                'To stop loop when there is no rows
                Exit Try
            End Try
            Con.Close()
        Catch edtb As OleDbException
            MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Try
        End Try
    End Sub
    'To enable editable TP Number
    Private Sub SelectionChanged(sender As Object, e As RoutedEventArgs)
        EditTPNumber.Visibility = Visibility.Visible
        EditChooseTPNumber.Visibility = Visibility.Collapsed
    End Sub

    'To load data into the form so that it'll be editable
    Private Sub LoadSelected(sender As Object, e As DependencyPropertyChangedEventArgs)
        If EditTPNumber.Visibility = Visibility.Collapsed Then

        Else
            EditTPNumber.Text = EditChooseTPNumber.SelectedValue

            Try
                'Connection to database
                Con.Open()

                Com = New OleDbCommand("SELECT TPNumber, Name, Contact, Email, Role FROM RegisteredStaff WHERE [TPNumber] ='" + EditTPNumber.Text + "'", Con)

                Reader = Com.ExecuteReader()
                Reader.Read()

                'Fills the fields with existing data
                EditName.Text = Reader.Item("Name")
                EditContact.Text = Reader.Item("Contact")
                EditEmail.Text = Reader.Item("Email")
                EditRole.Text = Reader.Item("Role")
                Con.Close()
            Catch edtb As OleDbException
                MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Exit Try
            End Try
        End If
    End Sub

    Private Sub EditAccount(sender As Object, e As RoutedEventArgs)
        If DoNotUpdatePassword.IsChecked = True Then
            'Turns off updating the password
            If EditName.Text = "" Or EditTPNumber.Text = "" Or EditContact.Text = "" Or EditEmail.Text = "" Or EditRole.Text = "" Then
                MessageBox.Show("Please fill in all the required information")
            Else
                Try
                    'Connection to database
                    Con.Open()

                    Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff WHERE TPNumber ='" + EditTPNumber.Text + "'", Con)

                    Reader = Com.ExecuteReader()
                    Reader.Read()

                    'Checks if TP Number existed in database
                    'Except if TP Number is the same
                    If (Reader.HasRows = False) Or EditTPNumber.Text = EditChooseTPNumber.SelectedValue Then

                        'Connection to database without updating password
                        Com = New OleDbCommand("UPDATE `RegisteredStaff` SET `TPNumber` = @TPNumber, `Name` = @Name, `Contact` = @Contact, `Email` = @Email, `Role` = @Role WHERE `TPNumber` ='" + EditChooseTPNumber.SelectedValue + "'", Con)
                        Com.Parameters.AddWithValue("@TPNumber", EditTPNumber.Text)
                        Com.Parameters.AddWithValue("@Name", EditName.Text)
                        Com.Parameters.AddWithValue("@Contact", EditContact.Text)
                        Com.Parameters.AddWithValue("@Email", EditEmail.Text)
                        Com.Parameters.AddWithValue("@Role", EditRole.Text)

                        Com.ExecuteNonQuery()

                        Dim addedData As Boolean = True

                        If addedData Then
                            EditName.Clear()
                            EditChooseTPNumber.SelectedIndex = -1
                            EditChooseTPNumber.Visibility = Visibility.Visible
                            EditTPNumber.Visibility = Visibility.Collapsed
                            EditPassword.Clear()
                            EditReconfirmPassword.Clear()
                            DoNotUpdatePassword.IsChecked = False
                            EditContact.Clear()
                            EditEmail.Clear()
                            EditRole.Text = ""
                            MessageBox.Show("Successful!")

                            addedData = False

                            'Closes Edit Account page
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
                    Else
                        MessageBox.Show("TP Number already existed" + vbCrLf + "Error Code: EACC001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    End If
                    Con.Close()
                Catch edtb As OleDbException
                    MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Exit Try
                End Try
            End If
        Else
            'Turns on updating the password
            If EditName.Text = "" Or EditTPNumber.Text = "" Or EditPassword.Password = "" Or EditReconfirmPassword.Password = "" Or EditContact.Text = "" Or EditEmail.Text = "" Or EditRole.Text = "" Then
                MessageBox.Show("Please fill in all the required information")
            Else
                Try
                    'Connection to database
                    Con.Open()

                    Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff WHERE TPNumber ='" + EditTPNumber.Text + "'", Con)

                    Reader = Com.ExecuteReader()
                    Reader.Read()

                    'Checks if TP Number existed in database
                    'Except if TP Number is the same
                    If (Reader.HasRows = False) Or EditTPNumber.Text = EditChooseTPNumber.SelectedValue Then
                        'Checks if password has a minimum of 8 characters
                        If EditPassword.Password.Length > 7 Then
                            'Warns if TP Number is the same as the password
                            If EditPassword.Password = EditTPNumber.Text Then
                                MessageBox.Show("Your TP Number and Password are the same. This will severely weaken the security." + vbCrLf + "Warning Code: WACC001", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning)
                            End If
                            'Checks if password is same with reconfirm password
                            If EditPassword.Password = EditReconfirmPassword.Password Then
                                'Encryption Service
                                Dim password = Encryption.HashString(EditPassword.Password)
                                Dim salt = Encryption.GenerateSalt
                                Dim hashedandsalted = Encryption.HashString(String.Format("{0}{1}", salt, password))

                                'Connection to database
                                Com = New OleDbCommand("UPDATE `RegisteredStaff` SET `TPNumber` = @TPNumber, `Name` = @Name, `Password` = @Password, `Contact` = @Contact, `Email` = @Email, `Role` = @Role, `Salt` = @Salt  WHERE `TPNumber` ='" + EditChooseTPNumber.SelectedValue + "'", Con)
                                Com.Parameters.AddWithValue("@TPNumber", EditTPNumber.Text)
                                Com.Parameters.AddWithValue("@Name", EditName.Text)
                                Com.Parameters.AddWithValue("@Password", hashedandsalted)
                                Com.Parameters.AddWithValue("@Contact", EditContact.Text)
                                Com.Parameters.AddWithValue("@Email", EditEmail.Text)
                                Com.Parameters.AddWithValue("@Role", EditRole.Text)
                                Com.Parameters.AddWithValue("@Salt", salt)

                                Com.ExecuteNonQuery()

                                Dim addedData As Boolean = True

                                If addedData Then
                                    EditName.Clear()
                                    EditChooseTPNumber.SelectedIndex = -1
                                    EditChooseTPNumber.Visibility = Visibility.Visible
                                    EditTPNumber.Visibility = Visibility.Collapsed
                                    EditPassword.Clear()
                                    EditReconfirmPassword.Clear()
                                    EditContact.Clear()
                                    EditEmail.Clear()
                                    EditRole.Text = ""
                                    MessageBox.Show("Successful!")

                                    addedData = False

                                    'Closes Edit Account page
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
                Catch edtb As OleDbException
                    MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Exit Try
                End Try
            End If
        End If
    End Sub

    'Ensures TP Number does not exist in database
    Private Sub TPNumberValidation(sender As Object, e As RoutedEventArgs)
        'Except if TP Number is the same
        Try
            If EditTPNumber.Text = "" Or EditTPNumber.Text = EditChooseTPNumber.SelectedValue Then

            Else
                Con.Open()
                'Connection to database
                Com = New OleDbCommand("SELECT [TPNumber] FROM RegisteredStaff WHERE TPNumber ='" + EditTPNumber.Text + "'", Con)

                Reader = Com.ExecuteReader()
                Reader.Read()

                If (Reader.HasRows = True) Then
                    TPNumberExisted.Visibility = Visibility.Visible
                Else
                    TPNumberExisted.Visibility = Visibility.Hidden
                End If
            End If
            Con.Close()
        Catch edtb As OleDbException
            MessageBox.Show("Problem connecting to the database" + vbCrLf + "Error Code: EDTB001", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Try
        End Try
    End Sub

    'Ensures only numbers and certain symbols are given
    Private Sub ContactNumberValidation(sender As Object, e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("[^0-9.-]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub PasswordValidation(sender As Object, e As RoutedEventArgs)
        'Ensures both password are the same
        If EditPassword.Password = EditReconfirmPassword.Password Then
            DoNotMatch.Visibility = Visibility.Collapsed
        Else
            DoNotMatch.Visibility = Visibility.Visible
        End If

        'Ensures password has a minimum of 8 characters
        If EditPassword.Password.Length < 8 Then
            InsufficientCharacters.Visibility = Visibility.Visible
        Else
            InsufficientCharacters.Visibility = Visibility.Collapsed
        End If

        'Warns if TP Number and the password are the same
        If EditPassword.Password = EditTPNumber.Text Then
            SameTPNumber.Visibility = Visibility.Visible
        Else
            SameTPNumber.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub DisableEditPassword(sender As Object, e As RoutedEventArgs)
        If DoNotUpdatePassword.IsChecked = True Then
            EditPassword.IsEnabled = False
            EditReconfirmPassword.IsEnabled = False
        Else
            EditPassword.IsEnabled = True
            EditReconfirmPassword.IsEnabled = True
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
