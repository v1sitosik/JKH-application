   M S B A M L   `   `   `  ����V  RЖКХ_Управление, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:ЖКХ_Управление.Controls$ЖКХ_Управление.Controls  5      _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aeHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.WpfMaterialDesignThemes.Wpf 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      ;    5ЖКХ_Управление.Controls.EmployeesControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Dlocal2clr-namespace:ЖКХ_Управление.Controls     5      FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      WpackIconHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.Wpf  5      0sys&clr-namespace:System;assembly=mscorlib  5      
�������  � Cursor$   ��5   !   .��5
      �� $��Hidden=�6   $��Disabled=�68   .��5   
   �� $��10q�6   .+�5      �� $3��-Управление сотрудниками��6   $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      
���   �5      � 5      $��0,0,0,10q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� -   5         ��Name$ ShowPasswordsCheckBox��5      $,��&Показать пароли (хеш)��5      $��Center=�5      $��0,0,20,0q�5      
���   �5       �� 5#      $��1��6    �� Orientation$ 
Horizontal=�6-   $��Right=�6F   .+�5$      �� -   5         ��Name$ SearchFieldComboBox��5$      $	��130��5%      $��48��5&      $��0,0,5,0q�5'      ���#F5F5F5
���   ����#CCCCCC "  MaterialDesignOutlinedComboBox#��[  5+      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Поле��5,       �� SelectedIndex$ 0��5-       �� 
IsEditable .  �� 
IsReadOnly . 	 �� Text	 50      �� *SelectedItem+
 �� RelativeSource
 �� *Self+.@�51      ��ID��6)   ��52      
Имя��6*   ��53      Фамилия��6.   ��54      
Логин��6,   ��55      Должность��60   56      �� 58      -   5         ��Name$ SearchTextBox��58      $	��160��59      $��48��6   ���#F5F5F5
���   ����#CCCCCC$��5q�6i   $ Поиск...��6t    ! MaterialDesignOutlinedTextBox#��[ 6�   6�   5:      5;      &   System.Windows.Controls.DataGrid  5=      -   5          Name$ EmployeesDataGrid��5=         AutoGenerateColumns .     SelectionMode$ Single=�5?         
IsReadOnly . $	��250��5A      $��0,0,0,10q�5B      
�������
���   ����#CCCCCC   Columns 5G      0  *System.Windows.Controls.DataGridTextColumn  5H      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *сотрудник_id+6V     5I      $ Имя��6)   $	 100 6N    66   �� *имя+6N     5J      $ Фамилия��6)   $	 100 6V    6:   �� *фамилия+6V     5K      $ 
Логин��6)   $	 100 6R    68   �� *
логин+6R     5L      $ Пароль��6)   $	 150 6T    69   �� *пароль+6T     5M      $ Должность��6)   $	 150 6Z    6<   �� *должность+6Z     5N      $ Дата приёма��6)   $	 120 6w    6>   �� *дата_приема+ �� StringFormat$ 
dd.MM.yyyy��6w   5P      � 5S      $��	0,10,0,10q�6   $��Center=�6+   .+�5T      �� -   5        $ NameTextBox��5T      $	��160��6/   $��48��6;   ���#F5F5F5
���   ����#CCCCCC$��5q�6�   $ Имя��6�   #��[ 6�   �� 5U      -   5        $ SurnameTextBox��5U      $	��150��62   $��48��6>   ���#F5F5F5
���   ����#CCCCCC$��5q�6�   $ Фамилия��6�   #��[ 6�   �� 5V      -   5        $ LoginTextBox��5V      $	��150��60   $��48��6<   ���#F5F5F5
���   ����#CCCCCC$��5q�6�   $ 
Логин��6�   #��[ 6�   G� 5W      -   5         G�Name$ PasswordBox��5W      $	��150��63   $��48��6?   ���#F5F5F5
���   ����#CCCCCC$��5q�6�   $ Пароль��6�    % !MaterialDesignOutlinedPasswordBox#��[ 6�   �� 5X      -	   5        $ PositionComboBox��5X      $	��185��65   $��48��6A   ���#F5F5F5
���   ����#CCCCCC$��5q�6�    �� DisplayMemberPath$ Название��6�   $) #Выберите должность��6�   #��[  6�   (  "System.Windows.Controls.DatePicker  5Y      -
   5          Name$ HireDatePicker��5Y      $	��160��65   $��48��6A   ���#F5F5F5
���   ����#CCCCCC$��5q�6�   $ Дата приёма��6�     MaterialDesignDatePicker#��[ 6�    � 	Resources 5Z       �� 
TargetType  CalendarButtonOverride �� Property �� Value �� 
TargetType'  !MaterialDesignThemes.Wpf.PackIcon   Kind��%  &	       �� 5[      " ��6    .�5\      ��  � �� 5]   "   �� 5^   &   " ��66   .R�5_   *   � .+�5`   .     $ Calendar=�6@   $��20��6P   $��20��6[   
���   �$��Center=�6z   $��Center=�6�   5a   +   5b   '   5d      5e      5f      5g      5h      � 5k      .+�5l      �� -   5        $��Добавить��5l      $	��100��6L   $��5q�6X   ���#0078D7
�������$��33��6�   �� 5m      -   5        $ ��Редактировать��5m      $	��130��6R   $��5q�6^   ���#0078D7
�������$��33��6�   �� 5n      -   5        $��Удалить��5n      $	��100��6N   $��5q�6Z   ���#E53935���#ff4e33
�������$��33��6�   �� 5o      -   5        $��Экспорт в Excel��5o      $	��150��6X   $��5q�6d   ���#0078D7
�������$��33��6�   �� 5p      -   5        $
��🔄��5p      $��50��6&   $��5q�61   ���#0078D7
�������$��33��6�   5q      5r      5s      5t      