Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Threading

Module GlobalVariables
    'For database
    Public ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Users\jozhuatwx\OneDrive - Asia Pacific University\Documents\Visual Studio 2017\Source\Repos\EasyKeyLibrarySystem\EasyKeyLibrarySystem\Database\LibraryDatabase.mdb"
    Public Con As OleDbConnection = New OleDbConnection(ConnectionString)
    Public Com As OleDbCommand
    Public Reader As OleDbDataReader

    Public dataAdapter As OleDbDataAdapter
    Public datatable As DataTable

    'For clock
    Public Timer As DispatcherTimer

    'For clock in/clock out timing
    Public CurrentDateTime As DateTime = DateTime.Now.ToString
    Public CurrentDate As Date = Date.Now.ToString("dd/MM/yyyy")
    Public CurrentTime As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm")

    'For time comparison
    Public ClockInTime, ClockOutTime, TimeNow, ClockInDateTime, ClockOutDateTime As String
    Public Start, PresentTime, Finish, RemoveDateClockInTime, RemoveDateClockOutTime As DateTime
    Public DifferenceDB, DifferenceDisplay As TimeSpan
    Public DecimalDifference, ReverseDecimalDifference As Double

    Public IndexH, IndexM, TimeH, TimeM, Shift As Integer
End Module
