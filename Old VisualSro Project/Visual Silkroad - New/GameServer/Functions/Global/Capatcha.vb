Namespace GameServer.Functions
    Module Capatcha

        Dim payload As Byte() = New Byte() {&H0, &H6D, &H2, &H65, &H2, &HC8, _
 &H32, &HC8, &H0, &H40, &H0, &H78, _
 &HDA, &HA5, &H54, &HBD, &H8E, &HD4, _
 &H30, &H10, &HFE, &H9C, &H4, &HD9, _
 &H20, &HB4, &H9, &HD4, &HAB, &H8D, _
 &HB, &HA, &H3A, &HDA, &H14, &HCB, _
 &H26, &H74, &H94, &H3C, &HC2, &HF1, _
 &H6, &H5B, &HAE, &HC4, &HE9, &HE2, _
 &HD3, &H51, &HD0, &HF1, &H2, &H14, _
 &H3C, &HC6, &H75, &HF8, &H24, &H6A, _
 &H78, &H81, &H93, &H88, &H44, &H41, _
 &HC7, &HA5, &HDC, &H83, &H28, &H66, _
 &H9C, &H1F, &HE7, &HEF, &HAE, &HC2, _
 &HD2, &H7A, &H9D, &HF9, &HE6, &HE7, _
 &H9B, &HF1, &H8C, &H1, &H4, &H80, _
 &HC0, &H19, &H46, &HCB, &H1B, &H9D, _
 &H55, &HB8, &H0, &HB2, &HF6, &HA3, _
 &H64, &HAD, &H82, &HDD, &HC8, &H7, _
 &HB4, &H53, &HD9, &HC9, &HDE, &H80, _
 &H5, &H63, &HBF, &HD0, &HDC, &H1D, _
 &H1F, &H2, &HD2, &H6, &HEE, &HD6, _
 &HD1, &H1, &H82, &H4D, &H4C, &H10, _
 &HF7, &H87, &H8, &HD3, &H53, &HE1, _
 &H4F, &H3E, &HD5, &H40, &HF9, &HB4, _
 &H13, &HCA, &HB9, &H69, &H9F, &H4C, _
 &H36, &HD, &HA1, &HDA, &H64, &H2, _
 &H44, &H33, &HC0, &HAE, &HDD, &HC8, _
 &H7B, &H84, &H8E, &H5C, &HE0, &H92, _
 &H51, &H4B, &H7F, &HD9, &HD1, &HC2, _
 &HAC, &HCF, &H23, &HEB, &H60, &HDA, _
 &H65, &H6C, &HB, &HC4, &HC6, &H45, _
 &H96, &HAD, &H75, &H61, &HD3, &HF4, _
 &H48, &H36, &H64, &H2F, &H29, &HDC, _
 &HD7, &HB4, &HCA, &HE9, &H2F, &H98, _
 &H93, &HE2, &HDA, &HDC, &H7E, &H79, _
 &H81, &H1, &H90, &H36, &HC5, &HE4, _
 &HC0, &HB9, &HFE, &HC9, &H92, &H92, _
 &H31, &H31, &HE8, &H96, &HC0, &HEA, _
 &HFA, &H8A, &HE1, &H80, &H15, &H25, _
 &H23, &H5C, &H32, &H85, &HE1, &H62, _
 &H7D, &H79, &HE1, &HA3, &HC0, &HCE, _
 &HD3, &HBC, &HA9, &H51, &H13, &HFD, _
 &H68, &H62, &H10, &HC0, &H9, &HD8, _
 &H4A, &H1C, &H23, &HD4, &H2D, &H7D, _
 &H95, &H9E, &HC4, &H30, &H37, &H86, _
 &HE1, &H12, &HB6, &H90, &H69, &HCD, _
 &H5A, &HF2, &H54, &HD6, &H2D, &HEA, _
 &H1F, &HC6, &HC7, &H5E, &H6D, &H28, _
 &H87, &HDA, &HEF, &HD2, &HD0, &H4F, _
 &H8, &H88, &H7F, &H6B, &H96, &H25, _
 &H14, &H1C, &HD5, &HDB, &HD3, &HBE, _
 &HE2, &HC, &H7B, &H6C, &HAE, &HCF, _
 &HFD, &H68, &H8F, &HF5, &HFB, &HCA, _
 &H57, &HEA, &H63, &HD7, &H21, &H7E, _
 &H7, &HD0, &HDF, &HBA, &HF2, &H49, _
 &HB1, &H5C, &H2, &H95, &H8F, &HA1, _
 &H50, &H5, &H43, &H82, &H90, &H62, _
 &H20, &HA9, &H88, &HD5, &H33, &H7, _
 &HE8, &H7, &H4, &HAC, &H2C, &H70, _
 &H4C, &H57, &HAE, &HE2, &HB4, &HED, _
 &HA9, &H7E, &HEA, &HE2, &H6F, &H8E, _
 &H32, &HFE, &HDE, &H28, &H3F, &H12, _
 &HD, &HF0, &H9A, &H54, &H43, &H98, _
 &H5F, &H79, &H19, &HEE, &H3E, &HF9, _
 &HFD, &H35, &H37, &H74, &HB9, &HE, _
 &H51, &HF1, &H32, &HC4, &H59, &HC2, _
 &HAD, &HB2, &HFD, &H45, &H84, &HEA, _
 &H1B, &H63, &H29, &H84, &H50, &HF1, _
 &HB7, &HA6, &H46, &HAE, &HBC, &H75, _
 &H4E, &HF2, &H94, &HE, &HEF, &HAE, _
 &HD9, &HA4, &H7B, &HFF, &H30, &HBA, _
 &HEC, &H2D, &H1D, &HE, &HEB, &HE6, _
 &HF6, &HA3, &HA1, &H1B, &H48, &H9F, _
 &H64, &H78, &HD9, &HB0, &HA5, &H4C, _
 &H7A, &HF0, &H8A, &H9A, &HC3, &HCA, _
 &H36, &HFC, &HB1, &HA5, &HE4, &HDA, _
 &H5A, &H5B, &HFF, &HD6, &HC2, &H18, _
 &HDB, &H3E, &H59, &HD7, &H86, &H41, _
 &H23, &HC7, &HBA, &HF0, &H4D, &HB, _
 &H10, &HA3, &HC6, &HBF, &H6A, &HE4, _
 &HB5, &H31, &HE7, &H86, &H61, &HD4, _
 &HD6, &H9F, &H73, &HF, &H2A, &H2F, _
 &H8C, &HC9, &H87, &H19, &H23, &H5, _
 &HA9, &H72, &HF2, &H6C, &HC2, &H8A, _
 &H11, &HF, &HE5, &HD, &H0, &H72, _
 &H28, &H63, &HD2, &H59, &H4F, &H52, _
 &H63, &HDB, &H62, &H6C, &HC4, &H74, _
 &H1C, &H19, &H1, &H56, &H8E, &HF, _
 &H76, &H7B, &HE, &H37, &H40, &H19, _
 &H82, &H7A, &H39, &H6E, &HB2, &H7D, _
 &H3, &HBC, &H40, &H8C, &HE2, &H21, _
 &HEA, &HBE, &HC4, &H64, &H9E, &HDB, _
 &H18, &H8A, &H84, &HEA, &HCD, &H1D, _
 &H73, &H1D, &HF5, &H9B, &HE8, &H4C, _
 &H33, &H37, &HBB, &HE4, &H55, &HF6, _
 &HBE, &H5F, &HCD, &HCC, &HE4, &HCC, _
 &H83, &H23, &H7D, &HE2, &H66, &H53, _
 &HE3, &HAE, &H25, &HD4, &HF2, &HA9, _
 &HBA, &H4F, &H30, &H8E, &H82, &H71, _
 &HAB, &HFD, &HCF, &HA, &H96, &HCF, _
 &HF7, &H3C, &HA4, &HCA, &HF0, &H74, _
 &H61, &HC3, &HEE, &H65, &H9, &HCC, _
 &HDE, &HD6, &HE1, &H4E, &HDA, &HA2, _
 &HFD, &H3, &H6D, &H3, &H9D, &H8B}

        Public Sub SendCaptcha(ByVal index_ As Integer)

            Dim packet As New PacketWriter
            packet.Create(ServerOpcodes.Capatcha)
            packet.Byte(payload)
            Server.Send(packet.GetBytes, index_)


        End Sub

    End Module
End Namespace
