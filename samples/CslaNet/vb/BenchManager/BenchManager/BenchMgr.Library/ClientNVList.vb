﻿Imports System.Data.SqlClient

<Serializable()> _
Public Class ClientNVList
  Inherits NameValueListBase(Of Integer, String)

  Public ReadOnly Property DefaultValue() As Integer
    Get
      If Me.Count > 0 Then
        Return Me(0).Key

      Else
        Return -1
      End If
    End Get
  End Property

  Private Shared _list As ClientNVList

  Public Shared Function GetList() As ClientNVList

    If _list Is Nothing Then
      _list = DataPortal.Fetch(Of ClientNVList)()
    End If
    Return _list

  End Function

  Public Shared Sub FlushCache()

    _list = Nothing

  End Sub

  Private Sub New()

    ' require use of factory methods

  End Sub

  Private Overloads Sub DataPortal_Fetch()

    Using cn As New SqlConnection(Database.SalesConnectionString)
      cn.Open()
      Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = System.Data.CommandType.Text
        cm.CommandText = "SELECT id,name FROM Client ORDER BY name"
        Using dr As New Csla.Data.SafeDataReader(cm.ExecuteReader)
          IsReadOnly = False
          While dr.Read
            Add(New NameValuePair(dr.GetInt32("id"), dr.GetString("name")))
          End While
          IsReadOnly = True
        End Using
      End Using
    End Using

  End Sub

End Class