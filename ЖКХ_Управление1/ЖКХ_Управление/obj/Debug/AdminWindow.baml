   M S B A M L   `   `   `  ����V  RЖКХ_Управление, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ЖКХ_УправлениеЖКХ_Управление  5   	   -    'ЖКХ_Управление.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;local)clr-namespace:ЖКХ_Управление     5   	     � Title$$  ЖКХ Админ-панель��5   	   $	��620��6"   $
��1110��6/    � WindowStartupLocation$ CenterScreen=�5   	   ���#FFF � 	Resources 5
         NavButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Triggers	 P� Property
 P� Value �� 
TargetName��%h  &	        �� 5   
   " ��6'   .�5      ��  � ��
 �����6+   �� 5       � ��
 ���� 6+   �� 5       � ��$ 0q�60   �� 5       � ��$ 16(�6)   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ Stretch=�6;   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ Border��6!   #��z��61   ._�5      �� $��Stretch=�6/   $��Center=�5   '   $
��10,5q�5   '   5       5      P� 5      	 �@J�IsMouseOver
 . . �5   "   �� $ Border��6)    � �� �#42A5F56S   5      5       5"      5#      5&      .��5)      � ���#FFF � ColumnDefinitions 5*   
   �� 5+      $	��180 �6   �� 5,      $��* �6   .+�51   
   ]� ���#1976D2��. $��2��6>   $��	0,0,900,0q�6R   .+�52      �� $	��Top=�6   .+�53      �� $��Меню��6   $��20(�6(   
�������$��
20,20,0,20q�6I   �� 54      -   5         ��Name$ EmployeesTab��54      $&�� 🧑‍💼 Сотрудники��6/   $��10q�6e   #��[  6r   $	��200��6�   $��50��6�   �� 55      -   5        $��🔧 Мастера��55      $��10q�6G   #��[  6T   $	��200��6|   $��50��6�   �� 56      -   5        $��👥 Клиенты��56      $��10q�6G   #��[  6T   $	��200��6|   $��50��6�   �� 57      -   5        $��🏷 Должности��57      $��10q�6K   #��[  6X   $	��200��6�   $��50��6�   �� 58      -   5        $.��(🛠 Категории мастеров��58      $��10q�6U   #��[  6b   $	��200��6�   $��50��6�   �� 59      -   5        $��🤖 NLP шаблоны��59      $��10q�6G   #��[  6T   $	��200��6|   $��50��6�   5:      �� 5<      -   5        $��⏎ Выйти��5<      $��Bottom=�5=      $��10q�5?      #��[  5@      $	��200��5A      $��50��5B      $��Bottom=�5C       � Cursor$
 Hand��6(   5D      � 5H   
   $��1��6   $��	 20 0 0 0q�6   .+�5I      �� -   5         ��Name$ MainContent��5I      5J      5K      5L      