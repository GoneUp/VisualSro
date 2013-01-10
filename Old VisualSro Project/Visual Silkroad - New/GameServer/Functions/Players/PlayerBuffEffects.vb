Imports SRFramework

Namespace Functions
    Module PlayerBuffEffects
        Public Function GetBuffDuration(refskill As RefSkill) As UInt64
            For Each effect In refskill.EffectList
                Select Case effect.EffectId
                    Case "dura"
                        Return effect.EffectParams(0)
                End Select
            Next

            Return 0
        End Function

        Public Sub ParseBuffEffects(refskill As RefSkill, Index_ As Integer)
            For Each effect In refskill.EffectList
                ParseBuffEffect(effect, Index_)
            Next
        End Sub

        Public Sub ParseBuffEffect(buffeffect As SkillEffect, Index_ As Integer)
            Select Case buffeffect.EffectId
                Case "heal"
                    UpdateHPMP(Index_)

                Case "hste"
                    'Speed Update
                    UpdateSpeeds(Index_)
            End Select
        End Sub
    End Module
End Namespace
