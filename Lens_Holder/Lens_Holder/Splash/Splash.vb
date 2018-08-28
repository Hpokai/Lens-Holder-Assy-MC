Imports System
Imports System.Windows.Forms
Imports System.Text
Imports System.Threading
Imports System.Collections.Generic

Namespace JGP_Splash
    Public Class Splasher
        Private MySplashForm As frmSplash
        Private MySplashThread As Thread

        Private Sub ShowThread()
            Application.Run(MySplashForm)
        End Sub

        Public Property mStatus As String
            Set(ByVal value As String)
                If MySplashForm Is Nothing Then
                    Return
                End If
                MySplashForm.Status = value
            End Set

            Get
                If MySplashForm Is Nothing Then
                    Return ""
                Else
                    Return MySplashForm.Status
                End If
            End Get
        End Property

        Public Sub Show(ByVal strName As String)
            If MySplashThread IsNot Nothing Then
                Return
            End If

            MySplashForm = New frmSplash()
            Me.mStatus = strName

            MySplashThread = New Thread(New ThreadStart(AddressOf ShowThread))
            MySplashThread.IsBackground = True
            MySplashThread.SetApartmentState(ApartmentState.STA)
            MySplashThread.Start()
        End Sub

        Public Sub Close()
            If MySplashThread Is Nothing Then
                Return
            End If
            If MySplashForm Is Nothing Then
                Return
            End If

            Try
                MySplashForm.Invoke(New MethodInvoker(AddressOf MySplashForm.Close))
            Catch ex As Exception

            End Try

            MySplashForm = Nothing
            MySplashThread = Nothing
        End Sub
    End Class
End Namespace