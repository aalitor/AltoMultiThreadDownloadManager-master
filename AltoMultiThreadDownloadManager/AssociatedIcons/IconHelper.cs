using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AltoMultiThreadDownloadManager.AssociatedIcons
{
	/// <summary>
	/// Provides static methods to read system icons for both folders and files.
	/// </summary>
	/// <example>
	/// <code>IconReader.GetFileIcon("c:\\general.xls");</code>
	/// </example>
	public class IconReader
	{
		/// <summary>
		/// Options to specify the size of icons to return.
		/// </summary>
		public enum IconSize
		{
			/// <summary>
			/// Specify large icon - 32 pixels by 32 pixels.
			/// </summary>
			Large = 0,
			/// <summary>
			/// Specify small icon - 16 pixels by 16 pixels.
			/// </summary>
			Small = 1
		}
        
		/// <summary>
		/// Options to specify whether folders should be in the open or closed state.
		/// </summary>
		public enum FolderType
		{
			/// <summary>
			/// Specify open folder.
			/// </summary>
			Open = 0,
			/// <summary>
			/// Specify closed folder.
			/// </summary>
			Closed = 1
		}

		/// <summary>
		/// Returns an icon for a given file - indicated by the name parameter.
		/// </summary>
		/// <param name="name">Pathname for file.</param>
		/// <param name="size">Large or small</param>
		/// <param name="linkOverlay">Whether to include the link icon</param>
		/// <returns>System.Drawing.Icon</returns>
		public static System.Drawing.Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
		{
			var shfi = new Shell32.SHFILEINFO();
			var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

			if (true == linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;
			
			/* Check the size specified for return. */
			if (IconSize.Small == size)
			{
				flags += Shell32.SHGFI_SMALLICON ;
			} 
			else 
			{
				flags += Shell32.SHGFI_LARGEICON ;
			}
			Shell32.SHGetFileInfo(	name, 
				Shell32.FILE_ATTRIBUTE_NORMAL, 
				ref shfi, 
				(uint) Marshal.SizeOf(shfi), 
				flags );

			// Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
			var icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
			User32.DestroyIcon( shfi.hIcon );		// Cleanup
			return icon;
		}

		/// <summary>
		/// Used to access system folder icons.
		/// </summary>
		/// <param name="size">Specify large or small icons.</param>
		/// <param name="folderType">Specify open or closed FolderType.</param>
		/// <returns>System.Drawing.Icon</returns>
		public static System.Drawing.Icon GetFolderIcon( IconSize size, FolderType folderType )
		{
			// Need to add size check, although errors generated at present!
			var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

			if (FolderType.Open == folderType)
			{
				flags += Shell32.SHGFI_OPENICON;
			}
			
			if (IconSize.Small == size)
			{
				flags += Shell32.SHGFI_SMALLICON;
			} 
			else 
			{
				flags += Shell32.SHGFI_LARGEICON;
			}

			// Get the folder icon
			var shfi = new Shell32.SHFILEINFO();
			Shell32.SHGetFileInfo(	null, 
				Shell32.FILE_ATTRIBUTE_DIRECTORY, 
				ref shfi, 
				(uint) Marshal.SizeOf(shfi), 
				flags );

			System.Drawing.Icon.FromHandle(shfi.hIcon);	// Load the icon from an HICON handle

			// Now clone the icon, so that it can be successfully stored in an ImageList
			var icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

			User32.DestroyIcon( shfi.hIcon );		// Cleanup
			return icon;
		}	
	}


	
}

