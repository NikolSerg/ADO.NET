   M S B A M L   `   `   `  ����I  EShopKindaThing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ShopKindaThingShopKindaThing  5   	       ShopKindaThing.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ShopKindaThing     5   	     � Title$  	Customers��5   	   $	��450��6   $	��800��6(    � 
ResizeMode$ NoResize=�64    � WindowStartupLocation$ CenterScreen=�6J   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ customersDataGrid��5
         SelectionMode$ Single=�6F      AutoGenerateColumns .  $��0,0,0,80q�6y      CanUserAddRows .     CanUserDeleteRows .  ��6.   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6%   
   
IsReadOnly
 . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *id+6H     5      $	 Surname��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6R    66   �� *	surname+6R     5      $	 
First Name��6%   $ 1* 6W    69   �� *	firstName+6W     5      $	 
Patronymic��6%   $ 1* 6X    69   �� *
patronymic+6X     5      $	 Phone Number��6%   $ 1* 6[    6;   �� *phoneNumber+6[     5      $	 E-Mail��6%   $ 1* 6O    65   �� *eMail+6O   5      �� 5   
   -   5         ��Name$ addCustomer��5      $��Add Customer��6&   $
��Left=�6=   $��	10,0,0,25q�6X   $��99��6k   $��30��6v   $��Bottom=�6�   6�   �� 5   
   -   5        $ deleteCustomer��5      $��Delete Customer��6)   $
��Left=�6C   $��
114,0,0,25q�6^   $��99��6r   {�.  $��30��6�   $��Bottom=�6�   �� 5   
   -   5         ��Name$ connectionTextBox��5      $
��Left=�6-   $��	331,0,0,0q�6H    �� TextWrapping$
 Wrap=�6[   $�� ��6o   $	��459��6w    �� 
IsReadOnly .  �� IsUndoEnabled .  $��80��6�   $��Bottom=�6�   �� 5   
   -   5        $ showOrdersButton��5      $��Show Orders��6+   $
��Left=�6A   $��
218,0,0,25q�6\   $��99��6p   $��30��6{   $��Bottom=�6�   {�.  6�   5      5      