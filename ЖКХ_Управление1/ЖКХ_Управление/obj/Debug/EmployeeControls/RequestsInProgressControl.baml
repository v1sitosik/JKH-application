   M S B A M L   `   `   `  ����  ЖКХ_УправлениеL    FЖКХ_Управление.EmployeeControls.RequestsInProgressControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      
�������.��5      �� $��10q�6   .+�5   
   �� $&�� Заявки в процессе��6   $��20(�5	      $
��Bold"�5
      $��0,0,0,10q�5      
���   �5      &   System.Windows.Controls.DataGrid  5   
   -   5           Name$  InProgressRequestsGrid��5         AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5      $	��350��5      $��0,0,0,10q�5      
�������
���   ����#CCCCCC   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Имя клиента��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 6R   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *имя+6R     5      $# Фамилия клиента��6%   $	 120 6Z    6>   �� *фамилия+6Z     5      $ 
Адрес��6%   $	 200 6N    64   �� *
адрес+6N     5      $ Запрос��6%   $	 250 6O    65   �� *
текст+6O     5      $ Категория��6%   $	 120 6V    68   �� *категория+6V     5      $ Этап��6%   $	 120 6L    63   �� *
этап+6L     5      $ Мастер��6%   $	 150 6P    65   �� *мастер+6P   5!      �� 5$   
   -   5        $%��Заменить мастера��5$      $	��180��5%      $��35��5&      ���#0078D7
�������5)      5*      5+      