Public Interface IScript

    Sub Initialize(ByVal Host As IHost)

    Sub Method1()
    Sub Method2()
    Sub Method3()
    Sub Method4()

End Interface

Public Interface IHost

    ReadOnly Property TextBox() As System.Windows.Forms.TextBox
    Sub ShowMessage(ByVal Message As String)

End Interface