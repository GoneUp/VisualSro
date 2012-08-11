Namespace Id_Gen
    Module ID_Generator
        '====================Unique ID's
        Private UniqueIdCounter As UInteger = 1

        Public Function GetUnqiueId() As UInteger
            Dim toreturn As UInteger = UniqueIdCounter
            If UniqueIdCounter < UInteger.MaxValue Then
                UniqueIdCounter += 1
            ElseIf UniqueIdCounter = UInteger.MaxValue Then
                UniqueIdCounter = 0
                Log.WriteSystemLog("Reached UniqueId Max")
            End If

            Return toreturn
        End Function

        '=================Charcater Id's
        Public Function GetNewCharId() As UInteger
            For NewID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To GameDB.Chars.Count - 1
                    If GameDB.Chars(c) IsNot Nothing Then
                        If GameDB.Chars(c).CharacterId = NewID Then
                            free = False
                        End If
                    End If
                Next

                If free = True Then
                    Return NewID
                End If
            Next

            Throw New Exception("Cannot find a new Char ID!")
        End Function

        '======================Skill Over ID's
        Private SkillOverIdCounter As UInteger = 10

        Public Function GetSkillOverId() As UInteger
            Dim toreturn As UInteger = SkillOverIdCounter
            If SkillOverIdCounter < UInteger.MaxValue Then
                SkillOverIdCounter += 1
            ElseIf SkillOverIdCounter = UInteger.MaxValue Then
                SkillOverIdCounter = 0
                Log.WriteSystemLog("Reached SkillOverId Max")
            End If

            Return toreturn
        End Function

        '======================Casting ID's
        Private CastingIdCounter As UInteger = 10

        Public Function GetCastingId() As UInteger
            Dim toreturn As UInteger = CastingIdCounter
            If CastingIdCounter < UInteger.MaxValue Then
                CastingIdCounter += 1
            ElseIf CastingIdCounter = UInteger.MaxValue Then
                CastingIdCounter = 0
                Log.WriteSystemLog("Reached CastingID Max")
            End If

            Return toreturn
        End Function

        '======================Exchange ID's
        Private ExchangeIdCounter As UInteger = 10

        Public Function GetExchangeId() As UInteger
            Dim toreturn As UInteger = ExchangeIdCounter
            If ExchangeIdCounter < UInteger.MaxValue Then
                ExchangeIdCounter += 1
            ElseIf ExchangeIdCounter = UInteger.MaxValue Then
                ExchangeIdCounter = 0
                Log.WriteSystemLog("Reached ExcahngeID Max")
            End If

            Return toreturn
        End Function

        '======================Stall ID's
        Private StallIdCounter As UInteger = 10

        Public Function GetStallId() As UInteger
            Dim toreturn As UInteger = StallIdCounter
            If StallIdCounter < UInteger.MaxValue Then
                StallIdCounter += 1
            ElseIf StallIdCounter = UInteger.MaxValue Then
                StallIdCounter = 0
                Log.WriteSystemLog("Reached StallID Max")
            End If

            Return toreturn
        End Function

        '======================Party ID's
        Private PartyIdCounter As UInteger = 10

        Public Function GetPartyId() As UInteger
            Dim toreturn As UInteger = PartyIdCounter
            If PartyIdCounter < UInteger.MaxValue Then
                PartyIdCounter += 1
            ElseIf PartyIdCounter = UInteger.MaxValue Then
                PartyIdCounter = 0
                Log.WriteSystemLog("Reached PartyId Max")
            End If

            Return toreturn
        End Function

        '======================Item ID's
        Private ItemIdCounter As UInt64 = 2

        Public Function GetItemId() As UInt64
            Dim toreturn As UInteger = ItemIdCounter
            If ItemIdCounter < UInt64.MaxValue Then
                ItemIdCounter += 1
            ElseIf ItemIdCounter = UInt64.MaxValue Then
                ItemIdCounter = 0
                Log.WriteSystemLog("Reached ItemID Max!!! FATAL")
                Server.Stop()
            End If

            Return toreturn
        End Function

        Public Sub SetItemInitalValue(ByVal ID As UInt64)
            ItemIdCounter = ID + 1
        End Sub
    End Module
End Namespace