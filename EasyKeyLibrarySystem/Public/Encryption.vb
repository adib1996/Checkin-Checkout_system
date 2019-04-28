Public Class Encryption
    Private crypotoServiceProvider As System.Security.Cryptography.RNGCryptoServiceProvider
    Public Shared Function HashString(str As String) As String
        Return EncryptString(str)
    End Function
    Public Shared Function GenerateSalt() As String
        Using crypotoServiceProvider As New System.Security.Cryptography.RNGCryptoServiceProvider
            Dim sb As New System.Text.StringBuilder()
            Dim data As Byte() = New Byte(4) {}
            For i = 0 To 6
                crypotoServiceProvider.GetBytes(data)
                Dim value As String = BitConverter.ToString(data, 0)
                sb.ToString()
            Next
            Return EncryptString(sb.ToString())
        End Using
    End Function
    Private Shared Function EncryptString(str As String) As String
        Dim bytes As Byte() = System.Text.Encoding.ASCII.GetBytes(str)
        Dim hashed = System.Security.Cryptography.SHA512.Create().ComputeHash(bytes)
        Return Convert.ToBase64String(hashed)
    End Function
End Class
