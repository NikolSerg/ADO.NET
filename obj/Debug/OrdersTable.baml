   M S B A M L   `   `   `  ����I  EShopKindaThing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ShopKindaThingShopKindaThing  5   	        ShopKindaThing.OrdersTable   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ShopKindaThing     5   	     � Title$  OrdersTable��5   	   $	��450��6   $	��498��6*   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ ordersDataGrid��5
         AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  $��0,0,0,88q�6�   ��6+   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Order ID��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *id+6N     5      $ Product Code��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 0.5* 6;    6H   �� *gCode+  5      $ Product Name��6%   $	 1* 6;    6F   �� *gName+5      �� 5   
   -   5        
 ��Name$
 addOrderButton��5      $��	New Order��6)   $
��Left=�6=   $��
22,372,0,0q�6X   $	��Top=�6l   $��30��6�   $	��112��6�   6�   �� 5   
   -   5        $
 deleteButton��5      $��Delete Order��6'   $
��Left=�6>   $��356,372,0,0q�6Y   $	��Top=�6n   $��30��6�   $	��112��6�   {�.  6�   5      5      