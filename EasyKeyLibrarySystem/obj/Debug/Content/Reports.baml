   M S B A M L   `   `   `  ����O  KEasyKeyLibrarySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:EasyKeyLibrarySystemEasyKeyLibrarySystem  5      "    EasyKeyLibrarySystem.Reports   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:EasyKeyLibrarySystem     5        N� 
Foreground
  ����� N� Title$ Reports��5
      .��5      � .+�5   
   �� $��35��6   $	��Top=�6!    �� Orientation$ 
Horizontal=�69   $��	20 5 20 0q�6R   .+�5      �� -   5         ��Name$ NameList��5      $	��175��6)   $��0 0 5 0q�65   $��12(�6F   6T   �� 5      -   5         ��Name$ GenerateButton��5      $��5 0 5 0q�6-      MaterialDesignFlatButton#���   6>   {�.  
�������.��5      �� $ 
Horizontal=�6   .+�5      (� $��20��6   $��20��6)   .��5      �� $��24��6!   $��24��6,   .+�5      F� �����PQ��` �@1� ��J ���  	=  �  	= � 	=  �   z  z � �  -1  z  z �P�  -1�yD
P-��� �PQ��� S0� �G�zT0@P�� �� � ��׀�[  � ���[ ��[ � � ��[ ���[ ��[ �`o� ��[ �� �f ���Q80@��v �@]� �ا � -1�ا � -1 	= �PQ��` 	
�������6�   5      5      �� 5      $��Generate��6    $��12(�60   $
��Thin"�6>   $��Center=�6P   $��5 0 0 0q�6k   5      5      �� 5      -   5        $ ExportButton��5      $��5 0 5 0q�6+   #���   6<   {�.  
�������.��5      �� $ 
Horizontal=�6   .+�5      (� $��20��6   $��20��6)   .��5      �� $��24��6!   $��24��6,   .+�5      F� ����Ѐ�[ �� Ӑ�J ��  	=  @,  	=  	= � 	=  -1׀�[ ��O �� �� р���O� -1 -1 �� �� � -1 z р�� �� �@]� �g5 ѠI@T� �@]� @T� ��B� `v� � $� `v� � $� �W&���� P����  -1�q=
ףp @P���� ��� 	
�������6�   5      5       �� 5!      $��Export to Excel��6    $��12(�67   $
��Thin"�6E   $��Center=�6W   $��5 0 0 0q�6r   5"      5#      5$      �� 5'   
   $��0 40 0 0q�6   $
��Auto=�6)   .��5(      &   System.Windows.Controls.DataGrid  -   5          Name$ ReportsDataGrid��5(      $��12(�60   $��5 0 5 0q�6>   
������    
IsReadOnly .    AutoGenerateColumns .     Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn	   Header$
	 Name��6)   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� *Name+  5+      $	 	TP Number��6)   
 6<   �� *
TPNumber+  5,      $	 Clock In��6)   
 6;   �� *ClockInTime+ �� StringFormat$ 	{0:HH:mm}��  5-      $	 	Clock Out��6)   
 6<   �� *ClockOutTime+$ 	{0:HH:mm}��  5.      $	 Duration (Hours)��6)   
 6C   �� *
Duration+  5/      $
	 Date��6)   
 67   �� *Date+$ {0:dd/MM/yyyy}��  50      $
	 Duty��6)   
 67   �� *Duty+52      53      54      55      