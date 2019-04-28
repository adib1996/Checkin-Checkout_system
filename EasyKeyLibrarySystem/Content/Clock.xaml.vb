Imports System.Data.OleDb
Imports System.Windows.Threading

Class Clock
    Private Sub Load(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        'Current Time Clock
        Timer = New DispatcherTimer

        'To prevent the 1 second delay
        CurrentTimeText.Text = DateTime.Now.ToString("HH:mm")
        Timer.Interval = New TimeSpan(0, 0, 1)

        AddHandler(Timer.Tick), AddressOf Clock
        Timer.Start()

        'To auto select the time
        IndexH = DateTime.Now.Hour - 8

        Select Case DateTime.Now.Minute
            Case 0 To 7
                IndexM = 0
            Case 8 To 22
                IndexM = 1
            Case 23 To 37
                IndexM = 2
            Case 38 To 52
                IndexM = 3
            Case 53 To 59
                IndexM = 0
                'To add an hour if auto selected minute is within this range
                IndexH += 1
        End Select

        If IndexH = 0 Then
            Select Case DateTime.Now.Minute
                Case 0 To 37
                    IndexM = 0
                Case 38 To 52
                    IndexM = 1
                Case 53 To 59
                    IndexM = 0
                    'To add an hour if auto selected minute is within this range
                    IndexH += 1
            End Select
        End If

        'Retrieves the TP Number, Name & Role
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                TPNumber.Text = CType(window, MainWindow).SMTPNumber.Text
            End If
        Next
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                Name.Text = CType(window, MainWindow).SMName.Text
            End If
        Next
        For Each window As Window In Application.Current.Windows
            If (window.GetType = GetType(MainWindow)) Then
                Role.Text = CType(window, MainWindow).SMRole.Text
            End If
        Next

        If Role.Text = "Trainee" Then
            ClockInDuty.Items.Add("Trainee")
        Else
            ClockInDuty.Items.Remove("Trainee")
        End If

        'Clears previously selected values
        ClockInHours.SelectedIndex = -1
        ClockInMinutes.SelectedIndex = -1
        ClockInDuty.SelectedIndex = -1
        ClockOutHours.SelectedIndex = -1
        ClockOutMinutes.SelectedIndex = -1

        ClockedInText.Visibility = Visibility.Hidden
        ClockedInTime.Visibility = Visibility.Hidden
        ClockedInHours.Visibility = Visibility.Hidden

        ClockedOutText.Visibility = Visibility.Hidden
        ClockedOutTime.Visibility = Visibility.Hidden
        ClockedOutHours.Visibility = Visibility.Hidden

        'Determine whether user as Clocked In/ Clocked Out
        Con.Open()

        Com = New OleDbCommand("SELECT [TPNumber], [ClockInTime], [Date], [Duty] FROM WorkReport WHERE `TPNumber` = @TPNumber AND Date = @Date", Con)
        Com.Parameters.AddWithValue("@TPNumber", TPNumber.Text)
        Com.Parameters.AddWithValue("@Date", CurrentDate)

        Reader = Com.ExecuteReader()
        Reader.Read()

        If Reader.HasRows = False Then
            'If user has not clocked in
            ClockInButton.IsEnabled = True
            ClockInHours.IsEnabled = True
            ClockInMinutes.IsEnabled = True

            'If user is trainee, duty options will not be available
            If Role.Text = "Trainee" Then
                ClockInDuty.SelectedIndex = 6
                ClockInDuty.IsEnabled = False
            Else
                ClockInDuty.IsEnabled = True
            End If

            'Auto-Select Closest Time
            ClockInHours.SelectedIndex = IndexH
            ClockInMinutes.SelectedIndex = IndexM

            'Prevents users from clocking out
            ClockOutButton.IsEnabled = False
            ClockOutHours.IsEnabled = False
            ClockOutMinutes.IsEnabled = False
        Else
            'If user has clocked in
            ClockInButton.IsEnabled = False
            ClockInHours.IsEnabled = False
            ClockInMinutes.IsEnabled = False
            ClockInDuty.IsEnabled = False
            ClockInDuty.SelectedItem = Reader.Item("Duty")

            'To remove date retrieved and set time to 24 hours
            RemoveDateClockInTime = Reader.Item("ClockInTime")
            ClockInTime = RemoveDateClockInTime.ToString("HH:mm")

            ClockedInTime.Text = ClockInTime
            ClockedInText.Visibility = Visibility.Visible
            ClockedInTime.Visibility = Visibility.Visible
            ClockedInHours.Visibility = Visibility.Visible

            'If user has clocked in, check clock out status
            If ClockInButton.IsEnabled = False Then
                'Connection to database
                Com = New OleDbCommand("SELECT [TPNumber], [ClockOutTime], [Date] FROM WorkReport WHERE `TPNumber` = @TPNumber AND Date = @Date AND ClockOutTime IS NULL", Con)
                Com.Parameters.AddWithValue("@TPNumber", TPNumber.Text)
                Com.Parameters.AddWithValue("@Date", CurrentDate)

                Reader = Com.ExecuteReader()
                Reader.Read()

                'If user has clocked in but not clocked out
                If Reader.HasRows = True Then
                    ClockOutButton.IsEnabled = True
                    ClockOutHours.IsEnabled = True
                    ClockOutMinutes.IsEnabled = True

                    'Auto-Select Closest Time
                    ClockOutHours.SelectedIndex = IndexH
                    ClockOutMinutes.SelectedIndex = IndexM
                Else
                    'If user has clocked in and clocked out
                    Com = New OleDbCommand("SELECT [TPNumber], [ClockOutTime], [Date] FROM WorkReport WHERE `TPNumber` = @TPNumber AND Date = @Date AND ClockOutTime IS NOT NULL", Con)
                    Com.Parameters.AddWithValue("@TPNumber", TPNumber.Text)
                    Com.Parameters.AddWithValue("@Date", CurrentDate)

                    Reader = Com.ExecuteReader()
                    Reader.Read()

                    'Prevent clocking out again
                    ClockOutButton.IsEnabled = False
                    ClockOutHours.IsEnabled = False
                    ClockOutMinutes.IsEnabled = False

                    'To remove date retrieved and set time to 24 hours
                    RemoveDateClockOutTime = Reader.Item("ClockOutTime")
                    ClockOutTime = RemoveDateClockOutTime.ToString("HH:mm")

                    ClockedOutTime.Text = clockouttime
                    ClockedOutText.Visibility = Visibility.Visible
                    ClockedOutTime.Visibility = Visibility.Visible
                    ClockedOutHours.Visibility = Visibility.Visible
                    ClockInOutDifference(Me, New RoutedEventArgs)

                    'Once the Library Assistant filled both clock in and clock out fields, the calculate hours button is made visible.
                    TotalHoursTodayGroup.Visibility = Visibility.Visible
                End If
            End If
        End If
        Con.Close()
    End Sub
    Private Sub Clock(sender As Object, e As EventArgs)
        CurrentTimeText.Text = DateTime.Now.ToString("HH:mm")
    End Sub

    Private Sub ClockIn(sender As Object, e As RoutedEventArgs)
        'Accepts values in the combo box that are not blank.
        If Not ClockInHours.Text = "" Or ClockInMinutes.Text = "" Then

            'Converts Clock In time and Current time for comparison. 
            ClockInTime = CStr(ClockInHours.Text + ":" + ClockInMinutes.Text)
            TimeNow = CStr(CurrentTimeText.Text)

            Start = New DateTime
            PresentTime = New DateTime
            Start = Convert.ToDateTime(ClockInTime)
            PresentTime = Convert.ToDateTime(TimeNow)
            'Sets 15 minutes gap
            PresentTime = PresentTime.AddMinutes(15)

            ClockInDateTime = CStr(CurrentDate + " " + ClockInTime)

            'System will prevent clocking in if Clock In time is greater than the Current Clock time 
            If Start > PresentTime Then
                MessageBox.Show("Clock-in time cannot be 15 minutes past current time", "Error")
            Else
                'If Clock In time is lesser than or equal to current clock time, clock in is enabled below.
                Dim dialog As MsgBoxResult

                dialog = MessageBox.Show("You are about to clock in at " + ClockInTime + ", are you sure?", "Notice", MessageBoxButton.YesNo)
                If dialog = MsgBoxResult.Yes Then

                    'Records into Database
                    Con.Open()
                    Com = New OleDbCommand("INSERT INTO `WorkReport` (`TPNumber`, `Name`, `ClockInTime`, `Date`, `Duty`) VALUES (@TPNumber,@Name,@ClockInTime,@Date,@Duty)", Con)

                    Com.Parameters.AddWithValue("@TPNumber", TPNumber.Text)
                    Com.Parameters.AddWithValue("@Name", Name.Text)
                    Com.Parameters.AddWithValue("@ClockInTime", ClockInDateTime)
                    Com.Parameters.AddWithValue("@Date", CurrentDate)
                    If Role.Text = "Trainee" Then
                        Com.Parameters.AddWithValue("@Duty", "Trainee")
                    Else
                        Com.Parameters.AddWithValue("@Duty", ClockInDuty.Text)
                    End If

                    Com.ExecuteNonQuery()
                    Con.Close()

                    'Enables clock out button and combo box and disables clock in combo box and buttons to prevent tampering
                    MessageBox.Show("You have clocked in at " + ClockInTime, "Notice")
                    ClockInHours.IsEnabled = False
                    ClockInMinutes.IsEnabled = False
                    ClockInButton.IsEnabled = False
                    ClockInDuty.IsEnabled = False
                    ClockOutHours.IsEnabled = True
                    ClockOutMinutes.IsEnabled = True
                    ClockOutButton.IsEnabled = True

                    'Displays clocked in time
                    ClockedInTime.Text = ClockInTime
                    ClockedInText.Visibility = Visibility.Visible
                    ClockedInTime.Visibility = Visibility.Visible
                    ClockedInHours.Visibility = Visibility.Visible
                ElseIf dialog = MsgBoxResult.No Then
                    'Cancels clock in
                    MessageBox.Show("clock-in cancelled", "Notice")
                End If
            End If
            'Denies clock in if combo box is blank
        Else MessageBox.Show("Please select time", "Error")
        End If
    End Sub

    Private Sub ClockOut(sender As Object, e As RoutedEventArgs)
        'Accepts clock out if value in combo box is NOT blank.
        If Not ClockOutHours.Text = "" Or ClockOutMinutes.Text = "" Then

            'Converts clock-in, clock-out and current time for comparison purposes
            ClockInTime = CStr(ClockedInTime.Text)
            ClockOutTime = CStr(ClockOutHours.Text + ":" + ClockOutMinutes.Text)
            TimeNow = CStr(CurrentTimeText.Text)

            Start = New DateTime
            Finish = New DateTime
            PresentTime = New DateTime

            Start = Convert.ToDateTime(ClockInTime)
            Finish = Convert.ToDateTime(ClockOutTime)
            PresentTime = Convert.ToDateTime(TimeNow)
            'Adds 15 minutes gap
            PresentTime = PresentTime.AddMinutes(15)

            ClockOutDateTime = CStr(CurrentDate + " " + ClockOutTime)

            If Start > Finish Then
                'An LA cannot clock out at a time earlier than his/her clock in time
                MessageBox.Show("Clock-out time cannot be earlier than clock-in time", "Error")
                ClockOutHours.SelectedIndex = -1
                ClockOutMinutes.SelectedIndex = -1
            ElseIf Start = Finish Then
                'An LA cannot make his/her clock out time equal to the clock-in time
                MessageBox.Show("Clock-out time and clock-in time cannot be the same", "Error")
                ClockOutHours.SelectedIndex = -1
                ClockOutMinutes.SelectedIndex = -1
            ElseIf Finish > PresentTime Then
                'Clock out is disabled if an LA clocks out at a time greater than the system clock time
                MessageBox.Show("Clock-out time cannot be 15 minutes past current time", "Error")
            Else
                'Displays clock-out window if the conditions are met
                Dim dialog As MessageBoxResult

                dialog = MessageBox.Show("You are about to clock out at " + ClockOutTime + ", are you sure?", "Notice", MessageBoxButton.YesNo)
                If dialog = MessageBoxResult.Yes Then
                    'Calculate the duration to be entered into database
                    ClockedOutTime.Text = ClockOutTime
                    ClockInOutDifference(Me, New RoutedEventArgs)

                    Con.Open()
                    'Records into Database
                    Com = New OleDbCommand("UPDATE `WorkReport` SET `ClockOutTime` = @ClockOutTime, `Duration` = @Duration  WHERE `TPNumber` = @TPNumber AND `Date` = @Date AND ClockOutTime IS NULL", Con)
                    Com.Parameters.AddWithValue("@ClockOutTime", ClockOutDateTime)
                    Com.Parameters.AddWithValue("@Duration", DecimalDifference)
                    Com.Parameters.AddWithValue("@TPNumber", TPNumber.Text)
                    Com.Parameters.AddWithValue("@Date", CurrentDate)

                    Com.ExecuteNonQuery()
                    Con.Close()

                    'Disables clock out combo box and button
                    MessageBox.Show("You have clocked out at " + ClockOutTime, "Notice")
                    ClockOutHours.IsEnabled = False
                    ClockOutMinutes.IsEnabled = False
                    ClockOutButton.IsEnabled = False

                    'Displays clocked out time
                    ClockedOutText.Visibility = Visibility.Visible
                    ClockedOutTime.Visibility = Visibility.Visible
                    ClockedOutHours.Visibility = Visibility.Visible


                    'Once the Library Assistant filled both clock in and clock out fields, the calculate hours button is made visible.
                    TotalHoursTodayGroup.Visibility = Visibility.Visible

                ElseIf dialog = MessageBoxResult.No Then
                    'Cancels clock out
                    MessageBox.Show("Clock out cancelled", "Notice")
                End If
            End If
        Else
            'Rejects clock out if combo box value is blank.
            MessageBox.Show("Please select time", "Error")
        End If
    End Sub
    Private Sub ClockInOutDifference(sender As Object, e As RoutedEventArgs)
        'Determines how many hours the LA has worked in the shift.
        ClockInTime = CStr(ClockedInTime.Text)
        ClockOutTime = CStr(ClockedOutTime.Text)

        Start = New DateTime
        Finish = New DateTime

        Start = Convert.ToDateTime(ClockInTime)
        Finish = Convert.ToDateTime(ClockOutTime)

        DifferenceDB = Finish.Subtract(Start)

        'Calculates time difference and also processes common sense about time.
        TotalHoursToday.Text = DifferenceDB.ToString("h\:mm")

        DecimalDifference = DifferenceDB.Hours + DifferenceDB.Minutes / 60
    End Sub

    Private Sub TimeValidation(sender As Object, e As SelectionChangedEventArgs)
        If ClockInHours.SelectedIndex = 0 Then
            ClockInMinutes.Items.RemoveAt(0)
            ClockInMinutes.Items.RemoveAt(0)
        End If
    End Sub
End Class
