   M S B A M L   `   `   `  ����  ЖКХ_УправлениеI    CЖКХ_Управление.MasterControls.ActiveAssignmentsControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      
�������.��5      �� $��10q�6   .+�5   
   �� $*��$Мои активные заявки��6   $��20(�5      $
��Bold"�5      $��0,0,0,10q�5	      
���   �5
      &   System.Windows.Controls.DataGrid  5   
   -   5           Name$  AssignedRequestsDataGrid��5         AutoGenerateColumns .     SelectionMode$ Single=�5         
IsReadOnly .    CanUserAddRows .  $��0,0,0,10q�5      ���#FAFAFA$	��400��5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Клиент��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Клиент+6P     5      $ 
Адрес��6%   $	 200 6N    64   �� *
Адрес+6N     5      $ Заявка��6%   $	 250 6O    65   �� *
Текст+6O     5      $ Категория��6%   $	 150 6V    68   �� *Категория+6V     5      $ Назначено��6%   $	 150 6[    68   �� *ДатаНазначения+6[     5      $ Статус��6%   $	 120 6Z    65   �� *" СтатусНазначения+6Z   5      �� 5   
   -   5        $0��*Отметить как выполнено��5      $	��220��5       $��33��5!      ���#0078D7
�������$
��Bold"�5$      $
��Left=�5%      5&      5'      