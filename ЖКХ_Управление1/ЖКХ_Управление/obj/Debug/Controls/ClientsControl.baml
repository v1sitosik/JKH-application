   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       ЖКХ_Управление9   3ЖКХ_Управление.Controls.ClientsControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      0sys&clr-namespace:System;assembly=mscorlib  5      
�������.��5      �� $
��Auto=�6   $��Disabled=�66   .��5	   
   �� $��10q�6   .+�5
      �� $-��'Управление клиентами��6   $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      
���   �5      � 5      $��0,0,0,10q�6     � ColumnDefinitions  5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� $��1��6    �� Orientation$ 
Horizontal=�6-   $��Right=�6F   .+�5      �� -   5         ��Name$ SearchFieldComboBox��5      $	��130��5      $��48��5      $��0,0,5,0q�5      ���#F5F5F5
���   ����#CCCCCC "  MaterialDesignOutlinedComboBox#��[  5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Поле��5        �� SelectedIndex$ 0��5!       �� 
IsEditable .  �� 
IsReadOnly .  �� Text 5$      �� *SelectedItem+ �� RelativeSource �� *Self+.@�5%      ��ID��6)   ��5&      
Имя��6*   ��5'      Фамилия��6.   ��5(      Телефон��6.   ��5)      	Email��6,   5*      �� 5,      -   5        	 ��Name$	 SearchTextBox��5,      $	��160��5-      $��48��5.      ���#F5F5F5
���   ����#CCCCCC$��5q�52      $ Поиск...��53       ! MaterialDesignOutlinedTextBox#��[ 54      55      56      57      &   System.Windows.Controls.DataGrid  5:      -   5        
  Name$
 ClientsDataGrid��5:         AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5=      $	��250��5>      $��0,0,0,10q�5?      
�������
���   ����#CCCCCC   Columns 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6S   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *клиент_id+6S     5F      $ Имя��6)   $	 100 6N    66   �� *имя+6N     5G      $ Фамилия��6)   $	 130 6V    6:   �� *фамилия+6V     5H      $ Телефон��6)   $	 130 6V    6:   �� *телефон+6V     5I      $ Email��6)   $	 190 6R    68   �� *email+6R     5J      $% Дата регистрации��6)   $	 150 6�    6C   �� *!дата_регистрации+ �� StringFormat$ 
dd.MM.yyyy��6�   5L      � 5O      $��	0,10,0,10q�6   .+�5P      �� -   5        $	 ClientNameTextBox��5P      $	��150��65   $��48��6A   $��5q�6M   ���#F5F5F5
���   ����#CCCCCC#��[ 5R      $ Имя��5S      �� 5T      -   5        $	 ClientSurnameTextBox��5T      $	��150��68   $��48��6D   $��5q�6P   ���#F5F5F5
���   ����#CCCCCC#��[ 5V      $ Фамилия��5W      �� 5X      -   5        $	 ClientPhoneTextBox��5X      $	��160��66   $��48��6B   $��5q�6N   ���#F5F5F5
���   ����#CCCCCC#��[ 5Z      $ Телефон��5[      �� 5\      -   5        $	 ClientEmailTextBox��5\      $	��200��66   $��48��6B   $��5q�6N   ���#F5F5F5
���   ����#CCCCCC#��[ 5^      $ Email��5_      (  "System.Windows.Controls.DatePicker  5`      -   5          Name$ RegistrationDatePicker��5`      $	��160��5a      $��48��6)   $��5q�65   ���#F5F5F5
���   ����#CCCCCC  MaterialDesignDatePicker#��[ 5c      $% Дата регистрации��5d      5e      � 5i      .+�5j      �� -	   5        $ ��Редактировать��5j      $	��130��5l      $��33��5m      $��5q�5n      ���#0078D7
�������5p      �� 5q      -
   5        $��Удалить��5q      $	��100��5s      $��33��5t      $��5q�5u      ���#E53935���#ff4e33
�������5x      �� 5y      -   5        $��Экспорт в Excel��5y      $	��150��5{      $��33��5|      $��5q�5}      ���#0078D7
�������5      �� 5�      -   5        $
��🔄��5�      $��50��5�      $��33��5�      $��5q�5�      ���#0078D7
�������5�      5�      5�      5�      5�      