Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Win32
Imports System.IO

Class Reports
    Private Sub Load(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        'Connection to database
        Con.Open()
        Com = New OleDbCommand("SELECT [Name] FROM RegisteredStaff WHERE Role = 'Trainee' OR Role = 'Library Assistant'", Con)

        Reader = Com.ExecuteReader()

        'To populate the combobox
        NameList.Items.Add("All Accounts")
        Try
            Do
                Reader.Read()
                Dim Names As String = Reader.Item("Name")
                NameList.Items.Add(Names)
            Loop
        Catch ex As InvalidOperationException
            'To stop loop when there is no rows
            Exit Try
        End Try
        Con.Close()
    End Sub

    Private Sub EnableGenerateandExport(sender As Object, e As RoutedEventArgs)
        GenerateButton.IsEnabled = True
        ExportButton.IsEnabled = True
    End Sub

    Private Sub GenerateReport(sender As Object, e As RoutedEventArgs)
        'Connection to the database
        Con.Open()

        If NameList.SelectedValue = "All Accounts" Then
            Com = New OleDbCommand("SELECT * FROM WorkReport", Con)
        Else
            Com = New OleDbCommand("SELECT * FROM WorkReport WHERE [Name] ='" + NameList.SelectedValue + "'", Con)
        End If

        dataAdapter = New OleDbDataAdapter(Com)
        datatable = New DataTable
        dataAdapter.Fill(datatable)
        ReportsDataGrid.ItemsSource = datatable.AsDataView
        Con.Close()
    End Sub

    Private Sub ExporttoExcel(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Feature currently under development")

        GenerateReport(Me, New RoutedEventArgs)

        'Create a new save file dialog
        Dim savefile As New SaveFileDialog

        'Selects all cells and copies to Windows clipboard
        ReportsDataGrid.SelectAllCells()
        ReportsDataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader
        ApplicationCommands.Copy.Execute(Nothing, ReportsDataGrid)
        Dim result As String = CType(Clipboard.GetData(DataFormats.CommaSeparatedValue), String)
        ReportsDataGrid.UnselectAllCells()


        'Setup the dialog
        'Title of the dialog
        savefile.Title = "Choose Location to Save " + NameList.SelectedValue + ".csv"
        'Directory, optional
        'savefile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments / AppDomain.CurrentDomain.BaseDirectory
        'File Name, includes file path
        savefile.FileName = NameList.SelectedValue

        'Filter
        savefile.Filter = "Comma-Separated Value|*.csv"

        'Show the dialog and execute the process
        If savefile.ShowDialog() = True Then
            Dim excelfile As StreamWriter = New StreamWriter(savefile.FileName)
            excelfile.WriteLine(result)
            excelfile.Close()
            ReportsDataGrid.ItemsSource = Nothing
            MessageBox.Show("Successful!")
        End If
    End Sub
End Class
