using System;
using UIKit;
using Xamarin.Auth;
using Newtonsoft.Json;
using Foundation;
using System.Json;

namespace XamarinAuth
{
	public partial class ViewController : UIViewController
	{
		partial void UIButton85_TouchUpInside(UIButton sender)
		{
			// https://developer.linkedin.com/my-apps
			var auth = new OAuth2Authenticator(
				 clientId: "77czt68vpsssvi", 				 clientSecret: "twNG31mvP7dhuQKJ", 				 scope: "r_basicprofile", 				 authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"), 				 redirectUrl: new Uri("https://www.youtube.com/c/HoussemDellai"), 				 accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken"));

			var ui = auth.GetUI();

			auth.Completed += LinkedInAuth_Completed;

			PresentViewController(ui, true, null);
		}

		async void LinkedInAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var request = new OAuth2Request( 					"GET", 					new Uri("https://api.linkedin.com/v1/people/~:(id,firstName,lastName,headline,picture-url,summary,educations,three-current-positions,honors-awards,site-standard-profile-request,location,api-standard-profile-request,phone-numbers)?" 						  + "format=json" 						  + "&oauth2_access_token=" 						  + e.Account.Properties["access_token"]), 					null, 					e.Account);

				var linkedInResponse = await request.GetResponseAsync(); 				var json = linkedInResponse.GetResponseText(); 				var linkedInUser = JsonValue.Parse(json);

				var name = linkedInUser["firstName"] + " " + linkedInUser["lastName"]; 				var id = linkedInUser["id"]; 				var description = linkedInUser["headline"]; 				var picture = linkedInUser["pictureUrl"];  				NameLabel.Text += name; 				IdLabel.Text += id; 				DescriptionLabel.Text = description; 				PictureImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(picture))); 
			}
		}

		partial void UIButton15_TouchUpInside(UIButton sender)
		{
			// https://developers.facebook.com/apps/
			var auth = new OAuth2Authenticator(
				clientId: "751294665026968", 				scope: "", 				authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"), 				redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

			var ui = auth.GetUI();

			auth.Completed += FacebookAuth_Completed;

			PresentViewController(ui, true, null);
		}

		async void FacebookAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var request = new OAuth2Request( 					"GET", 					new Uri("https://graph.facebook.com/me?fields=name,picture,cover,birthday"), 					null, 					e.Account); 
				var fbResponse = await request.GetResponseAsync();  				var fbUser = JsonValue.Parse(fbResponse.GetResponseText()); 
				var name = fbUser["name"];
				var id = fbUser["id"]; 				var picture = fbUser["picture"]["data"]["url"]; 				var cover = fbUser["cover"]["source"];

				NameLabel.Text += name; 				IdLabel.Text += id; 				PictureImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(picture))); 				CoverImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(cover))); 			}

			DismissViewController(true, null);
		}


		partial void UIButton41_TouchUpInside(UIButton sender)
		{
			// http://dev.twitter.com/apps
			var auth = new OAuth1Authenticator(
					consumerKey: "wls4oNSNjtUTaEhzJs825g", 					consumerSecret: "cawXz62nwLygGLCEQO1WeK6L2BbKIjI1pwxrRA9LRY", 					requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"), 					authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"), 					accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"), 					callbackUrl: new Uri("http://mobile.twitter.com"));
				
			var ui = auth.GetUI();

			auth.Completed += TwitterAuth_Completed;

			PresentViewController(ui, true, null);
		}

		async void TwitterAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var request = new OAuth1Request("GET",
							   new Uri("https://api.twitter.com/1.1/account/verify_credentials.json"),
							   null,
							   e.Account);

				var response = await request.GetResponseAsync();

				var json = response.GetResponseText();

				var twitterUser = JsonConvert.DeserializeObject<TwitterUser>(json);

				NameLabel.Text = twitterUser.name;
				IdLabel.Text = twitterUser.id.ToString();
				DescriptionLabel.Text = twitterUser.description; 				PictureImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(twitterUser.profile_image_url_https))); 				CoverImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(twitterUser.profile_banner_url))); 			}

			DismissViewController(true, null);
		}

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}



		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
