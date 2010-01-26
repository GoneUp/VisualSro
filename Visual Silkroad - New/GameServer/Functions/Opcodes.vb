Module Opcodes

    'All Opcodes for now

    'C --> S
    Public opLoginClientHandshake As UShort = "9000"
    Public opLoginClientPatchReq As UShort = "6100"
    Public opLoginClientInfoReq As UShort = "2001"

    'S --> C
    Public opLoginClientInfo As UShort = "0x2001" 'or "2001"?
    Public opLoginClientPatchInfo As UShort = "600D"

End Module
