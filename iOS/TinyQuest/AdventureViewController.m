//
//  ViewController.m
//  TinyQuest
//
//  Created by sato.daigo on 11/11/26.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import "AdventureViewController.h"
#import "ItemDialogController.h"

@implementation AdventureViewController
@synthesize itemDialogController, controlPanelView;

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle
- (void)setupInventory
{
    NSInteger count = 5;
    CGSize frameSize = scrollView.frame.size;
    scrollView.contentSize=CGSizeMake(frameSize.width, frameSize.height * count);
    scrollView.contentInset=UIEdgeInsetsMake(0.0,0.0,0.0,0.0);
    scrollView.delaysContentTouches = YES;

    // Setup all item buttons
    UIImage *image = [UIImage imageNamed:@"slots.png"];
    for (NSInteger i = 0; i < count; i++) 
    {
        UIImageView *imageView = [[UIImageView alloc] initWithImage:image];
        imageView.frame = CGRectMake(0,  i * frameSize.height, frameSize.width, frameSize.height);
        imageView.userInteractionEnabled = YES;
        for (NSInteger buttonIndex = 0; buttonIndex < 9; buttonIndex++) {
            NSInteger tx = buttonIndex % 3;
            NSInteger ty = buttonIndex / 3;
            
            UIButton *button = [UIButton buttonWithType:UIButtonTypeCustom];
            [button addTarget:self action:@selector(gotoProfile) forControlEvents:UIControlEventTouchUpInside];
            UIImage *image = [UIImage imageNamed:@"item1.png"];
            [button setImage:image forState:UIControlStateNormal];
            [button setFrame:CGRectMake(5 + tx * 62, 5 + ty * 54, 55, 45)];
            [imageView addSubview:button];
        }
        
        [scrollView addSubview:imageView];  
    }
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    [self setupInventory];
}

- (IBAction)gotoProfile
{
    if (!self.itemDialogController)
    {
        self.itemDialogController = [[ItemDialogController alloc] init];
        self.itemDialogController.delegate = self;
        [self.view addSubview:self.itemDialogController.view];
    }
    
    [self.itemDialogController slideIn];
    [self slideOut];
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[super viewWillDisappear:animated];
}

- (void)viewDidDisappear:(BOOL)animated
{
	[super viewDidDisappear:animated];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)slideIn
{
    self.controlPanelView.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.controlPanelView.frame = CGRectMake(0, self.controlPanelView.frame.origin.y, self.controlPanelView.frame.size.width, self.controlPanelView.frame.size.height);
    [UIView commitAnimations];
}

- (void)slideOut
{
    self.controlPanelView.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.controlPanelView.frame = CGRectMake(-320, self.controlPanelView.frame.origin.y, self.controlPanelView.frame.size.width, self.controlPanelView.frame.size.height);
    [UIView commitAnimations];
}

-(void)onSlideFinished:(NSString *)animationID finished:(NSNumber *)finished context:(void *)context
{
    self.controlPanelView.userInteractionEnabled = YES;
}
@end
