   M S B A M L   `   `   `  ����  ЖКХ_Управление;    5ЖКХ_Управление.Controls.PositionsControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      
�������.��5      �� .��5   
   �� $��10q�6   .+�5      �� $/��)Справочник должностей��6   $��20(�5	      $
��Bold"�5
      
���   �$��0,0,0,10q�5      &   System.Windows.Controls.DataGrid  5      -   5           Name$  PositionsDataGrid��5         AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5      $	��200��5      $��0,0,0,10q�5      
�������
���   ����#CCCCCC   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *должность_id+6V     5      $ Название��6)   $ * 6X    6;   �� *название+6X   5      � 5       $��	0,10,0,10q�6   .+�5!      �� -   5         ��Name$ PositionNameTextBox��5!      $	��200��5"      $��48��5#      $��5,0,20,0q�5$      ���#F5F5F5
���   ����#CCCCCC !  MaterialDesignOutlinedTextBox#��[  5(      )  #MaterialDesignThemes.Wpf.HintAssist	   Hint$)	 #Название должности��5)      5*      � 5+      $��0,0,0,10q�6   .+�5,      �� -   5        $��Добавить��5,      $	��100��5.      $��33��6%   $��5q�61   ���#0078D7
�������5/   .   �� 50      -   5        $ ��Редактировать��50      $	��130��52      $��33��6%   $��5q�61   ���#0078D7
�������53   .   �� 54      -   5        $��Удалить��54      $	��100��56      $��33��6%   $��5q�61   ���#E53935���#ff4e33
�������59      �� 5:      -   5        $
��🔄��5:      $��50��5<      $��33��6$   $��5q�60   ���#0078D7
�������5=   .   5>      5?      5@      5A      