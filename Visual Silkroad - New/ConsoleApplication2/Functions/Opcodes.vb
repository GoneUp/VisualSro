Module Opcodes

    'All Opcodes for now

    'C --> S
    Public opLoginClientHandshake As String = "9000"
    Public opLoginClientPatchReq As String = "6100"
    Public opLoginClientInfoReq As String = "2001"

    'S --> C
    Public opLoginClientInfo As String = "0x2001" 'or "2001"?
    Public opLoginClientPatchInfo As String = "600D"

End Module
